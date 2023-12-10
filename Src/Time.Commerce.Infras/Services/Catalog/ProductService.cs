using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using MongoDB.Driver;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Extensions;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IProductRepository _productRepository;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeRepository _productAttributeRepository;
        public ProductService(IMapper mapper, IWorkContext workContext, IProductRepository productRepository, IProductAttributeService productAttributeService, IProductAttributeRepository productAttributeRepository)
        {
            _mapper = mapper;
            _workContext = workContext;
            _productRepository = productRepository;
            _productAttributeService = productAttributeService;
            _productAttributeRepository = productAttributeRepository;
        }
        public async Task<ProductView> CreateAsync(CreateProductModel model, CancellationToken cancellationToken = default)
        {
            var product = _mapper.Map<Product>(model);
            product.SeName = await GetSeName(product);
            product.Stores.Add(model.StoreId);
            var currentId = _workContext.GetCurrentUserId();
            if(string.IsNullOrEmpty(product.VendorId))
                product.VendorId = currentId;
            product.CreatedBy = currentId;
            product.CreatedOn = DateTime.UtcNow;

            if (model.Categories is not null && model.Categories.Any())
            {
                var flag = 1;
                foreach (var i in model.Categories)
                {

                    product.ProductCategoryMapping.Add(new ProductCategoryMapping
                    {
                        CategoryId = i,
                        DisplayOrder = flag
                    });
                    flag++;
                }
            }

            if (model.Attributes is not null && model.Attributes.Any())
            {
                foreach (var att in model.Attributes)
                {
                    var atts = _mapper.Map<ProductAttributeMapping>(att);
                    product.ProductAttributeMapping.Add(atts);
                }
            }

            var productCreated = await _productRepository.InsertAsync(product);
            await UpdateAttributes(productCreated.ProductAttributeMapping);
            return _mapper.Map<ProductView>(productCreated);
        }

        public async Task<ProductView> UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(model.Id);

            if(product.Name != model.Name)
                product.SeName = await GetSeName(product);

            product.Name = model.Name;
            product.Sku = model.Sku;
            product.ShortDescription = model.ShortDescription;
            product.Location = model.Location;
            product.StartDateUtc = model.StartDateUtc;
            product.Description = model.Description;
            product.ShowOnHomePage = model.ShowOnHomePage;
            product.SubjectToAcl = model.SubjectToAcl;
            product.Gtin = model.Gtin;
            product.IsGiftCard = model.IsGiftCard;
            product.CallForPrice = model.CallForPrice;
            product.Price = model.Price;
            product.OldPrice = model.OldPrice;
            product.StartPrice = model.StartPrice;
            product.EndPrice = model.EndPrice;
            product.Weight = model.Weight;
            product.Length = model.Length;
            product.Width = model.Width;
            product.Height = model.Height;
            product.OrderMinimumQuantity = model.OrderMinimumQuantity;
            product.OrderMaximumQuantity = model.OrderMaximumQuantity;
            product.AllowCustomerReviews = model.AllowCustomerReviews;
            product.MarkAsNew = model.MarkAsNew;
            product.Published = model.Published;
            product.MetaKeywords = model.MetaKeywords;
            product.MetaDescription = model.MetaDescription;
            product.MetaTitle = model.MetaTitle;
            product.UpdatedOn = DateTime.UtcNow;
            product.UpdatedBy = _workContext.GetCurrentUserId();

            product.BrandId = model.BrandId;
            product.SpeakerId = model.SpeakerId;
            product.ProductCategoryMapping.Clear();
            if (model.Categories.Any())
            {
                var flag = 1;
                foreach (var i in model.Categories)
                {

                    product.ProductCategoryMapping.Add(new ProductCategoryMapping
                    {
                        CategoryId = i,
                        DisplayOrder = flag
                    });
                    flag++;
                }
            }

            product.Notes = model.Notes;

            var productUpdated = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductView>(productUpdated);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await _productRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _productRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<ProductView>> FindAsync(ProductQueryModel model, CancellationToken cancellationToken = default)
        {
            var data = await FindProductsAsync(model, cancellationToken);
            return new PageableView<ProductView>
            (
                model.PageIndex,
                model.PageSize,
                data.TotalItems,
                _mapper.Map<IEnumerable<ProductView>>(data.Items).ToList()
            );
        }

        public async Task<PageableView<Product>> FindProductsAsync(ProductQueryModel model, CancellationToken cancellationToken = default)
        {
            var filter = GetProductFilter(model);

            var query = new DataFilterPaging<Product> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };
            else
                query.Sort = new List<SortOption> { new SortOption { Field = nameof(Product.CreatedOn), Ascending = false } };

            var products = await _productRepository.CountAndQueryAsync(query);
            return new PageableView<Product>
            (
                model.PageIndex,
                model.PageSize,
                products.Total,
                products.Data.ToList()
            );
        }

        public async Task<ProductDetailsView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<ProductDetailsView>(await GetByIdAsync(id));

        public async Task<ProductDetailsView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            =>  _mapper.Map<ProductDetailsView>(await _productRepository.FindOneAsync(x => x.SeName == slug));

        public ProductVariant? GetSpecificVariant(Product product, IList<CustomAttribute> attributesOptions)
        {
            if (attributesOptions is not null && attributesOptions.Any() && product.Variants is not null && product.Variants.Any())
            {
                ProductVariant productVariant = null;
                foreach (var variant in product.Variants)
                {
                    var attributes = variant.Attributes;
                    if (attributes is not null && attributes.Any())
                    {
                        if (attributesOptions.All(i => attributes.Any(a => a.Key == i.Key && a.Value == i.Value)))
                        {
                            productVariant = variant;
                            break;
                        }
                    }
                }
                return productVariant;
            }

            return null;
        }

        public async Task<ProductView> AddImages(string id, List<ImageModel> model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            if (!model.Any())
                return _mapper.Map<ProductView>(product);
               
            foreach(var img in model)
            {
                product.Images.Add(new Image
                {
                    Id = img.Id,
                    Path = img.Path,
                    AlternateText = img.AlternateText,
                });
            }
            return await UpdateProductAsync(product);
        }

        public async Task<ProductView> DeleteImage(string id, string imageId, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            var image = product.Images.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                product.Images.Remove(image);
            }
            return await UpdateProductAsync(product);
        }

        public async Task<bool> CopyProduct(CopyProductModel model, CancellationToken cancellationToken = default)
        {
            var productCopy = await GetByIdAsync(model.ProductCopyId);
            var newProduct = new Product
            {
                Name = model.Name,
                Sku = productCopy.Sku,
                ShortDescription = productCopy.ShortDescription,
                Description = productCopy.Description,
                ShowOnHomePage = productCopy.ShowOnHomePage,
                SubjectToAcl = productCopy.SubjectToAcl,
                Gtin = productCopy.Gtin,
                IsGiftCard = productCopy.IsGiftCard,
                CallForPrice = productCopy.CallForPrice,
                Price = productCopy.Price,
                OldPrice = productCopy.OldPrice,
                StartPrice = productCopy.StartPrice,
                EndPrice = productCopy.EndPrice,
                Weight = productCopy.Weight,
                Length = productCopy.Length,
                Width = productCopy.Width,
                Height = productCopy.Height,
                OrderMinimumQuantity = productCopy.OrderMinimumQuantity,
                OrderMaximumQuantity = productCopy.OrderMaximumQuantity,
                AllowCustomerReviews = productCopy.AllowCustomerReviews,
                MarkAsNew = productCopy.MarkAsNew,
                Published = model.Published,
                MetaKeywords = productCopy.MetaKeywords,
                MetaDescription = productCopy.MetaDescription,
                MetaTitle = productCopy.MetaTitle,
                CreatedBy = _workContext.GetCurrentUserId(),
                CreatedOn = DateTime.UtcNow,
                BrandId = productCopy.BrandId,
                Stores = productCopy.Stores,
                Locales = productCopy.Locales
            };

            if (productCopy.ProductCategoryMapping.Any())
                foreach (var i in productCopy.ProductCategoryMapping)
                    newProduct.ProductCategoryMapping.Add(i);

            if (productCopy.ProductAttributeMapping.Any())
                foreach (var i in productCopy.ProductAttributeMapping)
                    newProduct.ProductAttributeMapping.Add(i);

            if(model.CopyImage)
                foreach (var i in productCopy.Images)
                    newProduct.Images.Add(i);

            newProduct.SeName = await GetSeName(newProduct);

            var productCreated = await _productRepository.InsertAsync(newProduct);
            return productCreated is not null;
        }

        #region Attributes Methods
        public async Task<ProductView> AddAttribute(string id, ProductAddAttributeModel model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            if (!product.ProductAttributeMapping.Any(x => x.Name == model.Name))
            {
                var att = _mapper.Map<ProductAttributeMapping>(model);
                product.ProductAttributeMapping.Add(att);
            }

            product.Variants.Clear();

            return await UpdateProductAsync(product, true);
        }

        public async Task<ProductView> UpdateAttribute(string id, ProductUpdateAttributeModel model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            if (!product.ProductAttributeMapping.Any(x => x.Id == model?.Id))
                return null;

            product.ProductAttributeMapping.Remove(product.ProductAttributeMapping.FirstOrDefault(x => x.Id == model?.Id));
            var att = _mapper.Map<ProductAttributeMapping>(model);
            product.ProductAttributeMapping.Add(att);
            return await UpdateProductAsync(product, true);
        }

        public async Task<ProductView> DeleteAttribute(string id, string attributeId, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            if (!product.ProductAttributeMapping.Any(x => x.Id == attributeId))
                return null;

            product.ProductAttributeMapping.Remove(product.ProductAttributeMapping.FirstOrDefault(x => x.Id == attributeId));
            product.Variants.Clear();
            return await UpdateProductAsync(product);
        }

        public List<KeyValuePair<string, string>> GetAttributeControlType(CancellationToken cancellationToken = default)
            => EnumExtension.GetEnumValuesAsStringWithDescription<AttributeControlType>();
        #endregion

        #region Variants Methods
        private bool CheckProductVariantExist(List<ProductVariant> variants, ProductAddVariantModel model)
        {
            if (variants.Any())
            {
                foreach (var variant in variants)
                    if (model.Attributes.All(x => variant.Attributes.Any(y => x.Key == y.Key && x.Value == y.Value)))
                        return true;
            }
            return false;
        }

        public async Task<ProductView> AddVariant(string id, ProductAddVariantModel model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            if(CheckProductVariantExist(product.Variants.ToList(), model))
                throw new BadRequestException(nameof(CommonErrors.VARIANT_EXISTED), CommonErrors.VARIANT_EXISTED);
            product.Variants.Add(new ProductVariant
            {
                ImageId = model.ImageId,
                Price = model.Price,
                Attributes = _mapper.Map<List<GenericAttribute>>(model.Attributes)
            });
            return await UpdateProductAsync(product);
        }

        public async Task<ProductView> UpdateVariant(string id, ProductUpdateVariantModel model, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            var updateVariant = product.Variants?.First(x => x.Id == model.Id);
            updateVariant.ImageId = model.ImageId;
            updateVariant.Price = model.Price;
            updateVariant.Sku = model.Sku;
            updateVariant.Name = model.Name;
            updateVariant.StockQuantity = model.StockQuantity;
            return await UpdateProductAsync(product);
        }

        public async Task<ProductView> DeleteVariant(string id, string variantId, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);
            product.Variants.Remove(product.Variants.FirstOrDefault(x => x.Id == variantId));
            return await UpdateProductAsync(product);
        }

        public async Task<ProductView> ApplyVariants(string id, IList<ProductAddVariantModel> variants, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id);

            if (variants is not null && variants.Any())
            {
                product.Variants.Clear();
                foreach (var v in variants)
                {
                    var m = _mapper.Map<ProductVariant>(v);
                    product.Variants.Add(m);
                }
            }

            var productUpdated = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductView>(productUpdated);
        }
        #endregion


        #region Private Methods
        private async Task<Product> GetByIdAsync(string id)
            => await _productRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        public async Task<IEnumerable<ProductOverViewView>> FindProductsOverViewAsync(ProductQueryModel model, CancellationToken cancellationToken = default)
        {
            #region query style
            //var match = new BsonDocument
            //{
            //    {
            //        "$match" , new BsonDocument
            //        {
            //            { "_id", id }
            //        }
            //    }
            //};
            //var lookup1 = new BsonDocument
            //    {
            //        {
            //            "$lookup", new BsonDocument
            //            {
            //                { "from",  "Category" },
            //                { "localField",  "ProductCategoryMapping.CategoryId" },
            //                {"foreignField",  "_id" },
            //                { "as",  "Categories" }
            //            }
            //        }
            //    };
            //var lookup2 = new BsonDocument
            //    {
            //        {
            //            "$lookup", new BsonDocument
            //            {
            //                { "from",  "Brand" },
            //                { "localField",  "BrandId" },
            //                {"foreignField",  "_id" },
            //                { "as",  "Brand" }
            //            }
            //        }
            //    };
            //var unwind = new BsonDocument
            //    {
            //        {
            //            "$unwind", new BsonDocument{
            //                            { "path", "$Brand" },
            //                            { "preserveNullAndEmptyArrays", true }
            //                        }
            //        }
            //    };
            //var query = _productRepository.Collection
            //.Aggregate<ProductOverView>(new[] { lookup1, lookup2, unwind });
            //var products = await query.ToListAsync();
            #endregion
            var filter = GetProductFilter(model);
            var query = _productRepository.Collection
           .Aggregate()
           .Match(filter)
           .Lookup("Category", "ProductCategoryMapping.CategoryId", "_id", "Categories")
           .Lookup("Brand", "BrandId", "_id", "Brand").Unwind("Brand", new AggregateUnwindOptions<ProductOverView> { PreserveNullAndEmptyArrays = true}).As<ProductOverView>();
            var products = await query.ToListAsync();
            return _mapper.Map<IEnumerable<ProductOverViewView>>(products);
        }

        public async Task<ProductOverViewView> GetProductOverViewBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            Expression<Func<Product, bool>> filter = null;
            filter = x => x.SeName == slug;
            return await GetProductOverView(filter);
        }

        public async Task<ProductOverViewView> GetProductOverViewByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            Expression<Func<Product, bool>> filter = null;
            filter = x => x.Id == id;
            return await GetProductOverView(filter);
        }

        private async Task<string> GetSeName(Product product)
        {
            if (!string.IsNullOrEmpty(product.SeName) && !await _productRepository.AnyAsync(x => x.SeName == product.SeName))
                return product.SeName;

            product.SeName = SlugHelper.GenerateSlug(product.Name);

            int i = 2;
            var tempSeName = product.SeName;
            while (true)
            {
                if (await _productRepository.AnyAsync(x => x.SeName == tempSeName))
                {
                    tempSeName = string.Format("{0}-{1}", product.SeName, i);
                    i++;
                }
                else
                {
                    break;
                }

            }
            return tempSeName;
        }

        private Expression<Func<Product, bool>> GetProductFilter(ProductQueryModel model)
        {
            Expression<Func<Product, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText) ||
                x.MetaDescription.ToLower().Contains(searchText) ||
                x.MetaKeywords.ToLower().Contains(searchText) ||
                x.MetaTitle.ToLower().Contains(searchText);

            var userId = _workContext.GetCurrentUserId();
            if(!string.IsNullOrEmpty(userId))
                filter = filter.And(x => x.CreatedBy == userId);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (!string.IsNullOrWhiteSpace(model?.SpeakerId))
                filter = filter.And(x => x.SpeakerId != null && x.SpeakerId == model.SpeakerId);

            if(model.FromDate is not null && model.ToDate is not null)
            {
                filter = filter.And(x => x.StartDateUtc.HasValue && x.StartDateUtc >= model.FromDate && x.StartDateUtc <= model.ToDate);
            }

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);

            if (model.Ids is not null &&  model.Ids.Any())
                filter = filter.And(x => model.Ids.Contains(x.Id));
            return filter;
        }

        public async Task<ProductOverViewView> GetProductOverView(Expression<Func<Product, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _productRepository.Collection
           .Aggregate()
           .Match(filter)
           .Lookup("Category", "ProductCategoryMapping.CategoryId", "_id", "Categories")
           .Lookup("Brand", "BrandId", "_id", "Brand").Unwind("Brand", new AggregateUnwindOptions<ProductOverView> { PreserveNullAndEmptyArrays = true }).As<ProductOverView>();
            var product = await query.FirstOrDefaultAsync();
            return _mapper.Map<ProductOverViewView>(product);
        }

        public async Task<ProductView> UpdateProductAsync(Product product, bool? updateAttributes = false)
        {
            if (updateAttributes == true)
                await UpdateAttributes(product.ProductAttributeMapping);

            return _mapper.Map<ProductView>(await _productRepository.UpdateAsync(product));
        }

        private async Task UpdateAttributes(IEnumerable<ProductAttributeMapping> productsAttributes)
        {
            if (productsAttributes is not null && productsAttributes.Any())
            {
                var idsToCheck = productsAttributes.Select(x => x.AttributeId).ToList();
                var existingAttributes = await _productAttributeService.GetAll();
                if (existingAttributes is not null)
                {
                    foreach(var id in idsToCheck)
                    {
                        var att = productsAttributes.FirstOrDefault(p => p.AttributeId == id);
                        if (att is not null && att.Values is not null)
                        {
                            var existingAttribute = existingAttributes.FirstOrDefault(x => x.Id == id);
                            existingAttribute.OptionValues ??= new List<ProductAttributeOptionValue>();
                            foreach (var option in att.Values)
                            {
                                if(!existingAttribute.OptionValues.Any(o => o.Name == option))
                                    existingAttribute.OptionValues.Add(new ProductAttributeOptionValue
                                    {
                                        Name = option
                                    });
                            }
                            await _productAttributeRepository.UpdateAsync(existingAttribute);
                        }
                    }
                }
            }
        }
        #endregion
    }
}