using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Extensions;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.Core.Extensions;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Sales;
using Time.Commerce.Domains.Repositories.Sales;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Sales
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
        private readonly ISpeakerService _speakerService;
        public OrderService(IMapper mapper, IWorkContext workContext, IOrderRepository orderRepository, IShoppingCartService shoppingCartService, IProductService productService, ISpeakerService speakerService)
        {
            _mapper = mapper;
            _workContext = workContext;
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _speakerService = speakerService;
        }
        public async Task<OrderView> CreateAsync(CreateOrderModel model, CancellationToken cancellationToken = default)
            => await CreateOrder(model, cancellationToken);

        public async Task<OrderView> CreateWithShoppingCartAsync(string cartId, CreateOrderModel model, CancellationToken cancellationToken = default)
        {
            var cart = await _shoppingCartService.GetShoppingCartViewWithProductInforAsync(cartId);
            if (cart is null)
                throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

            model.OrderItems = new List<OrderItemModel>();
            foreach (var item in cart.Items)
            {
                model.OrderItems.Add(new OrderItemModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    PictureThumbnailUrl = item.Product.Image?.Path,
                    Quantity = item.Quantity,
                    PriceInclTax = item.Price,
                    PriceExclTax = item.Price,
                });
            }
            return await CreateOrder(model, cancellationToken);
        }

        public async Task<OrderView> UpdateAsync(UpdateOrderModel model, CancellationToken cancellationToken = default)
        {
            var order = await GetByIdAsync(model.Id);
            if(order.OrderStatus == OrderStatus.Complete)
                throw new BadRequestException(nameof(CommonErrors.ORDER_ALREADY_COMPLETE), CommonErrors.ORDER_ALREADY_COMPLETE);

            order.Note = model.Note;
            order.StoreName = model.StoreName;
            order.CustomerId = model.CustomerId;
            order.CompanyName = model.CompanyName;
            order.CustomerEmail = model.CustomerEmail;
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UrlReferrer = model.UrlReferrer;
            order.CustomerCurrencyCode = model.CustomerCurrencyCode;
            order.CustomerLanguageId = model.CustomerLanguageId;
            order.CustomerIp = _workContext.GetCurrentIpAddress();
            order.AffiliateId = model.AffiliateId;
            order.AffiliateName = model.AffiliateName;
            order.AllowStoringCreditCardNumber = model.AllowStoringCreditCardNumber;
            order.CardType = model.CardType;
            order.CardName = model.CardName;
            order.CardNumber = model.CardNumber;
            order.MaskedCreditCardNumber = model.MaskedCreditCardNumber;
            order.CardCvv2 = model.CardCvv2;
            order.CardExpirationMonth = model.CardExpirationMonth;
            order.CardExpirationYear = model.CardExpirationYear;
            order.OwnerId = model.OwnerId;
            order.PickUpInStore = model.PickUpInStore;
            order.OrderStatus = (OrderStatus)model.OrderStatus;
            order.ShippingStatus = (ShippingStatus)model.ShippingStatus;
            order.PaymentStatus = (PaymentStatus)model.PaymentStatus;
            order.PaymentMethod = model.PaymentMethod;
            order.PrimaryCurrencyCode = model.PrimaryCurrencyCode;
            order.BillingAddress = _mapper.Map<Address>(model.BillingAddress);
            order.ShippingAddress = _mapper.Map<Address>(model.ShippingAddress);

            order.OrderTags.Clear();
            foreach (var tag in model.OrderTags)
                order.OrderTags.Add(tag);

            order.OrderItems.Clear();

            decimal subTotal = 0;
            foreach (var item in model.OrderItems)
            {
                order.OrderItems.Add(_mapper.Map<OrderItem>(item));
                subTotal += item.PriceInclTax * item.Quantity;
            }
            order.OrderSubtotalInclTax = subTotal;
            order.OrderSubtotalExclTax = subTotal;
            //todo: calculate tax ...
            order.OrderTotal = subTotal;


            order.UpdatedOn = DateTime.UtcNow;
            order.UpdatedBy = _workContext.GetCurrentUserId();
            var orderUpdated = await _orderRepository.UpdateAsync(order);
          
            return _mapper.Map<OrderView>(orderUpdated);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await _orderRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
            =>  await _orderRepository.DeleteManyAsync(ids);

        public async Task<PageableView<OrderView>> FindAsync(OrderQueryModel model, CancellationToken cancellationToken = default)
        {
            var orders = await GetAllAsync(model);
            return new PageableView<OrderView>
                (
                    model.PageIndex,
                    model.PageSize,
                    orders.Total,
                    _mapper.Map<IEnumerable<OrderView>>(orders.Data).ToList()
                );
        }
       
        public async Task<OrdersSummaryView> GetOrdersSummaryAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var fromDate = now.AddDays(-1);
            fromDate = fromDate.StartOfTheDay();
            now = now.EndOfTheDay();

            Expression<Func<Order, bool>> filter = o => o.OrderStatus == OrderStatus.Complete &&
                o.CreatedOn >= fromDate && o.CreatedOn <= now;

            var orders = await _orderRepository.FindAsync(filter);
            var todayOrders = orders.Where(o => o.CreatedOn >= now.StartOfTheDay());
            var lastdayOrders = orders.Where(o => o.CreatedOn < now.StartOfTheDay());

            return new OrdersSummaryView
            {
                TodaySales = todayOrders.Sum(o => o.OrderTotal),
                TodayOrders = todayOrders.Count(),
                LastDaySales = lastdayOrders.Sum(o => o.OrderTotal),
                LastDayOrders = lastdayOrders.Count(),
            };
        }
        
        public async Task<OrderView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<OrderView>(await GetByIdAsync(id));

        public OrderStatusesView GetOrderStatuses(CancellationToken cancellationToken = default)
        {
            return new OrderStatusesView
            {
                OrderStatus = EnumExtension.GetEnumValuesAsString<OrderStatus>(),
                PaymentStatus = EnumExtension.GetEnumValuesAsString<PaymentStatus>(),
                ShippingStatus = EnumExtension.GetEnumValuesAsString<ShippingStatus>()
            };
        }
        #region Private Methods
        private async Task<OrderView> CreateOrder(CreateOrderModel model, CancellationToken cancellationToken = default)
        {
            var order = _mapper.Map<Order>(model);
            order.CustomerIp = _workContext.GetCurrentIpAddress();
            order.OrderNumber = RandomOrderNumber();

            if (order.ShippingStatus == ShippingStatus.WaitingForDeliver && order.PaymentStatus == PaymentStatus.Pending)
                order.OrderStatus = OrderStatus.Pending;
            else if (order.ShippingStatus == ShippingStatus.Shipped && order.PaymentStatus == PaymentStatus.Paid)
                order.OrderStatus = OrderStatus.Complete;
            else
                order.OrderStatus = OrderStatus.Processing;

            decimal subTotal = 0;
            var products = await _productService.FindAsync(new Contracts.Models.Catalog.ProductQueryModel
            {
                Ids = order.OrderItems.Select(x => x.ProductId).ToList()
            }, cancellationToken);
            var speakers = await _speakerService.FindAsync(new Contracts.Models.Identity.SpeakerQueryModel
            {
                Ids = products?.Items?.Select(p => p.SpeakerId).ToList()
            });
            foreach (var item in order.OrderItems)
            {
                var product = products.Items.FirstOrDefault(x => x.Id == item.ProductId);
                item.PictureThumbnailUrl = speakers?.Items?.FirstOrDefault(s => s.Id == product.SpeakerId)?.Image?.Path;
                item.ProductId = product?.Id;
                item.ProductName = product?.Name;
                var price = product?.Price ?? 0;
                item.PriceExclTax = price;
                item.PriceInclTax = price;
                subTotal += price * item.Quantity;
            }
            order.OrderSubtotalInclTax = subTotal;
            order.OrderSubtotalExclTax = subTotal;

            order.OrderShippingInclTax = 0;
            order.OrderShippingExclTax = 0;

            order.OrderDiscount = 0;
            order.OrderSubTotalDiscountExclTax = 0;
            order.OrderSubTotalDiscountInclTax = 0;

            //todo: calculate tax ...
            order.OrderTotal = subTotal;


            order.CreatedBy = _workContext.GetCurrentUserId();
            order.CreatedOn = DateTime.UtcNow;
            var orderCreated = await _orderRepository.InsertAsync(order);
            return _mapper.Map<OrderView>(orderCreated);
        }

        private async Task<DataFilterPagingResult<Order>> GetAllAsync(OrderQueryModel model)
        {
            Expression<Func<Order, bool>> filter = null;
            filter = x => true;
            if (!string.IsNullOrWhiteSpace(model?.SearchText))
            {
                var searchText = model?.SearchText?.ToLower();
                filter =  filter.And(x => (
                (!string.IsNullOrEmpty(x.OrderNumber) && x.OrderNumber.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(x.CustomerEmail) && x.CustomerEmail.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(x.FirstName) && x.FirstName.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(x.LastName) && x.LastName.ToLower().Contains(searchText))
                ));

            }

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.StoreId == model.StoreId);

            if (model?.OrderStatus is not null)
                filter = filter.And(x => x.OrderStatus == (OrderStatus)model.OrderStatus);

            if (model?.PaymentStatus is not null)
                filter = filter.And(x => x.PaymentStatus == (PaymentStatus)model.PaymentStatus);

            if (model?.ShippingStatus is not null)
                filter = filter.And(x => x.ShippingStatus == (ShippingStatus)model.ShippingStatus);

            if(model.FromDate is not null && model.ToDate is not null)
                filter = filter.And(x => x.CreatedOn >= model.FromDate.Value.StartOfTheDay() && x.CreatedOn <= model.ToDate.Value.EndOfTheDay());

            var query = new DataFilterPaging<Order> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _orderRepository.CountAndQueryAsync(query);
        }
        private async Task<Order> GetByIdAsync(string id)
            => await _orderRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private string RandomOrderNumber()
            => StringExtensions.RandomString(12);
        #endregion
    }
}
