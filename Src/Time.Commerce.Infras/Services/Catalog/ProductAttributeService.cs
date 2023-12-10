using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Catalog
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IDistributedCacheService _distributedCacheService;
        public ProductAttributeService(IMapper mapper, IWorkContext workContext, IProductAttributeRepository productAttributeRepository, IDistributedCacheService distributedCacheService)
        {
            _mapper = mapper;
            _workContext = workContext;
            _productAttributeRepository = productAttributeRepository;
            _distributedCacheService = distributedCacheService;
        }
        public async Task<ProductAttributeView> CreateAsync(CreateProductAttributeModel model, CancellationToken cancellationToken = default)
        {
            var productAttribute = _mapper.Map<ProductAttribute>(model);
            productAttribute.Stores.Add(model.StoreId);
            productAttribute.CreatedBy = _workContext.GetCurrentUserId();
            productAttribute.CreatedOn = DateTime.UtcNow;
            var productAttributeCreated = await _productAttributeRepository.InsertAsync(productAttribute);
            await DeleteAllCacheAsync(model.StoreId);
            return _mapper.Map<ProductAttributeView>(productAttributeCreated);
        }

        public async Task<ProductAttributeView> UpdateAsync(UpdateProductAttributeModel model, CancellationToken cancellationToken = default)
        {
            var productAttribute = await GetByIdAsync(model.Id);

            productAttribute.Name = model.Name;
            productAttribute.IsColorAttribute = model.IsColorAttribute;
            productAttribute.Description = model.Description;
            productAttribute.Type = model.Type;
            productAttribute.Published = model.Published;
            productAttribute.OptionValues = _mapper.Map<IList<ProductAttributeOptionValue>>(model.OptionValues);
            productAttribute.UpdatedOn = DateTime.UtcNow;
            productAttribute.UpdatedBy = _workContext.GetCurrentUserId();
            var productAttributeUpdated = await _productAttributeRepository.UpdateAsync(productAttribute);
            foreach (var store in productAttribute.Stores)
                await DeleteAllCacheAsync(store);
            return _mapper.Map<ProductAttributeView>(productAttributeUpdated);
        }

        public async Task<bool> BulkUpdateAsync(IEnumerable<ProductAttribute> attributes, IEnumerable<string>? updateFields,
            CancellationToken cancellationToken = default)
        {
            if(updateFields is null || !updateFields.Any())
                updateFields = new List<string>() {  "Name" };
            return await _productAttributeRepository.BulkUpdateAsync(attributes, updateFields);
        }
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(id);
            foreach(var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _productAttributeRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(ids[0]);
            foreach (var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _productAttributeRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<ProductAttributeView>> FindAsync(ProductAttributeQueryModel model, CancellationToken cancellationToken = default)
        {
            var productAttributes = await GetAllAsync(model);
            return new PageableView<ProductAttributeView>
                (
                    model.PageIndex,
                    model.PageSize,
                    productAttributes.Total,
                    _mapper.Map<IEnumerable<ProductAttributeView>>(productAttributes.Data).ToList()
                );
        }

        public async Task<ProductAttributeView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<ProductAttributeView>(await GetByIdAsync(id));

        public async Task<IEnumerable<ProductAttribute>> GetAll(CancellationToken cancellationToken = default)
        {
            Expression<Func<ProductAttribute, bool>> filter = null;
            filter = x => true;
            return await _productAttributeRepository.FillterAsync(filter);
        }

        #region Private Methods
        private async Task<DataFilterPagingResult<ProductAttribute>> GetAllAsync(ProductAttributeQueryModel model)
        {
            Expression<Func<ProductAttribute, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);

            if (model.Ids is not null && model.Ids.Any())
            {
                filter = filter.And(x => model.Ids.Contains(x.Id));
            }

            var query = new DataFilterPaging<ProductAttribute> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

           

            return await _productAttributeRepository.CountAndQueryAsync(query);
        }
        private async Task<ProductAttribute> GetByIdAsync(string id)
            => await _productAttributeRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task DeleteAllCacheAsync(string storeId)
        {
            await _distributedCacheService.RemoveAsync(GetCacheKey(ProductAttributeCacheEnum.GetAllWithParent, storeId));
            await _distributedCacheService.RemoveAsync(GetCacheKey(ProductAttributeCacheEnum.GetAllWithRecursive, storeId));
        }

        private string GetCacheKey(ProductAttributeCacheEnum type, string storeId)
            => type == ProductAttributeCacheEnum.GetAllWithParent ? $"timecommerce.productAttribute.getallwithparent.{storeId}" : $"timecommerce.productAttribute.getallwithrecursive.{storeId}";

       
        #endregion

        private enum ProductAttributeCacheEnum
        {
            GetAllWithParent,
            GetAllWithRecursive
        }
    }
}
