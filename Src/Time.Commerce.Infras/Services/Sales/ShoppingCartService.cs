using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using DocumentFormat.OpenXml.VariantTypes;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Sales;
using Time.Commerce.Domains.Repositories.Sales;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Sales
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductService _productService;
        private readonly IPricingService _pricingService;
        public ShoppingCartService(IMapper mapper, IShoppingCartRepository shoppingCartRepository, IProductService productService, IPricingService pricingService)
        {
            _mapper = mapper;
            _shoppingCartRepository = shoppingCartRepository;
            _productService = productService;
            _pricingService = pricingService;
        }
        public async Task<ShoppingCartView> CreateAsync(CreateShoppingCartModel model, CancellationToken cancellationToken = default)
        {
            var products = await _productService.FindAsync(new Contracts.Models.Catalog.ProductQueryModel
            {
                Ids = model.Items.Select(x => x.ProductId).ToList(), 
            });
            if(products.Items.Count != model.Items.Count)
                throw new BadRequestException(nameof(CommonErrors.PRODUCT_NOT_FOUND), CommonErrors.PRODUCT_NOT_FOUND);
            var shoppingCart = _mapper.Map<ShoppingCart>(model);
            shoppingCart.CreatedOn = DateTime.UtcNow;
            var shoppingCartCreated = await _shoppingCartRepository.InsertAsync(shoppingCart);
            return _mapper.Map<ShoppingCartView>(shoppingCartCreated);
        }

        public async Task<ShoppingCartView> UpdateAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default)
        {
            var shoppingCart = await GetByIdAsync(model.Id);
            shoppingCart.CustomerId = model.CustomerId;
            
            var oldItems = shoppingCart.Items;
            foreach(var item in model.Items)
            {
                var oldItem = oldItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                bool isUpdate = false;
                if (oldItem is not null)
                {
                    var equals = oldItem.Attributes.All(o => item.Attributes.Any(i => i.Key == o.Key && i.Value == o.Value));
                    if(equals)
                    {
                        oldItems.FirstOrDefault(x => x.ProductId == item.ProductId).Quantity = item.Quantity;
                        isUpdate = true;
                    }
                }
                
                if(!isUpdate)
                    oldItems.Add(_mapper.Map<ShoppingCartItem>(item));
            }
            var shoppingCartUpdated = await _shoppingCartRepository.UpdateAsync(shoppingCart);
            return _mapper.Map<ShoppingCartView>(shoppingCartUpdated);
        }

        public async Task<ShoppingCartView> CreateOrUpdateAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(model.Id))
                return await CreateAsync(model, cancellationToken);
            
            return await UpdateAsync(model, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await _shoppingCartRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
            => await _shoppingCartRepository.DeleteManyAsync(ids);

        public async Task<PageableView<ShoppingCartView>> FindAsync(ShoppingCartQueryModel model, CancellationToken cancellationToken = default)
        {
            var shoppingCarts = await GetAllAsync(model);
            return new PageableView<ShoppingCartView>
                (
                    model.PageIndex,
                    model.PageSize,
                    shoppingCarts.Total,
                    _mapper.Map<IEnumerable<ShoppingCartView>>(shoppingCarts.Data).ToList()
                );
        }

        public async Task<ShoppingCartView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<ShoppingCartView>(await GetByIdAsync(id));

        #region Private Methods
        private async Task<DataFilterPagingResult<ShoppingCart>> GetAllAsync(ShoppingCartQueryModel model)
        {
            Expression<Func<ShoppingCart, bool>> filter = null;
            filter = x => true;

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.StoreId == model.StoreId);

            var query = new DataFilterPaging<ShoppingCart> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _shoppingCartRepository.CountAndQueryAsync(query);
        }
        private async Task<ShoppingCart> GetByIdAsync(string id)
            => await _shoppingCartRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        public async Task<string> CreateEmptyCartAsync(string storeId, string customerId = null, CancellationToken cancellationToken = default)
            => (await _shoppingCartRepository.InsertAsync(new ShoppingCart { CreatedOn = DateTime.UtcNow, StoreId = storeId, CustomerId = customerId, ShoppingCartType = nameof(ShoppingCartType.ShoppingCart) })).Id;

        public async Task<ShoppingCartView> AddProductsToCartAsync(string cartId, List<ShoppingCartItemModel> items, CancellationToken cancellationToken = default)
        {
            var products = await _productService.FindAsync(new Contracts.Models.Catalog.ProductQueryModel
            {
                Ids = items.Select(x => x.ProductId).ToList(),
            });
            if (products.Items.Count != items.Count)
                throw new BadRequestException(nameof(CommonErrors.PRODUCT_NOT_FOUND), CommonErrors.PRODUCT_NOT_FOUND);
            var shoppingCart = await GetByIdAsync(cartId);
            shoppingCart.UpdatedOn = DateTime.UtcNow;
            shoppingCart.Items ??= new List<ShoppingCartItem>();

            var oldItems = shoppingCart.Items;
            foreach (var item in items)
            {
                bool isUpdate = false;
                ShoppingCartItem existingItem = null;
                if (item.Attributes is not null && item.Attributes.Any())
                    existingItem = oldItems.FirstOrDefault(x => x.ProductId == item.ProductId && x.Attributes.All(o => item.Attributes.Any(i => i.Key == o.Key && i.Value == o.Value)));
                else
                    existingItem = oldItems.FirstOrDefault(x => x.ProductId == item.ProductId && (x.Attributes == null || !x.Attributes.Any()));

                if (existingItem is not null)
                {
                    existingItem.Quantity += item.Quantity;
                    isUpdate = true;
                }

                if (!isUpdate)
                    oldItems.Add(_mapper.Map<ShoppingCartItem>(item));
            }

            var shoppingCartUpdated= await _shoppingCartRepository.UpdateAsync(shoppingCart);
            return await GetShoppingCartViewWithProductInforAsync(cartId);
        }

        public async Task<ShoppingCartView> UpdateCartItemsAsync(string cartId, List<ShoppingCartItemModel> items, CancellationToken cancellationToken = default)
        {
            var shoppingCart = await _shoppingCartRepository.GetByIdAsync(cartId);
            shoppingCart.UpdatedOn = DateTime.UtcNow;
            shoppingCart.Items ??= new List<ShoppingCartItem>();
            var oldItems = shoppingCart.Items;
            foreach (var item in items)
            {
                var existingItem = oldItems.FirstOrDefault(x => x.Id == item.Id);
                if(existingItem is not null)
                    existingItem.Quantity = item.Quantity;
            }

            var shoppingCartUpdated = await _shoppingCartRepository.UpdateAsync(shoppingCart);
            return await GetShoppingCartViewWithProductInforAsync(cartId);
        }

        public async Task<ShoppingCartView> RemoveCartItemsAsync(string cartId, string cartItem, CancellationToken cancellationToken = default)
        {
            var shoppingCart = await _shoppingCartRepository.GetByIdAsync(cartId);
            shoppingCart.UpdatedOn = DateTime.UtcNow;

            var deleteItem = shoppingCart.Items.First(x => x.Id == cartItem);
            shoppingCart.Items.Remove(deleteItem);
            var shoppingCartUpdated = await _shoppingCartRepository.UpdateAsync(shoppingCart);
            return await GetShoppingCartViewWithProductInforAsync(cartId);
        }

        public async Task<ShoppingCartView?> GetShoppingCartViewWithProductInforAsync(string cartId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(cartId)) return null;

            var shoppingCart = await _shoppingCartRepository.GetByIdAsync(cartId);

            if (shoppingCart is null) return null;


            shoppingCart.Items ??= new List<ShoppingCartItem>();
            var shoppingCartView = _mapper.Map<ShoppingCartView>(shoppingCart);
            if (shoppingCart.Items.Any())
            {
                var productIds = shoppingCart.Items.Select(x => x.ProductId).ToList();
                var products = await _productService.FindProductsAsync(new Contracts.Models.Catalog.ProductQueryModel
                {
                    Ids = productIds,
                    PageSize = productIds.Count
                });
                foreach (var item in shoppingCartView.Items)
                {
                    var product = products.Items.First(x => x.Id == item.ProductId);
                    item.Product = _mapper.Map<ProductSimpleView>(product);
                    item.Price = item.Product.Price;
                    var productVariant = _productService.GetSpecificVariant(product, _mapper.Map<IList<CustomAttribute>>(item.Attributes));

                    if(productVariant != null)
                    {
                        if (productVariant.Price > 0)
                            item.Price = productVariant.Price;

                        var image = product.Images.FirstOrDefault(i => i.Id == productVariant.ImageId);
                        if(image is not null)
                            item.Product.Image = _mapper.Map<ImageView>(image);
                    }
                }
            }
            return _pricingService.CalculateCartTotalAsync(shoppingCartView);
        }
        #endregion
    }
}
