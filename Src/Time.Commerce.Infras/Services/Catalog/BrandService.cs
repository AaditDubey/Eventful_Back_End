using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Repositories.Catalog;
using Time.Commerce.Infras.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Catalog
{
    public class BrandService : IBrandService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IBrandRepository _brandRepository;
        private readonly IDistributedCacheService _distributedCacheService;
        public BrandService(IMapper mapper, IWorkContext workContext, IBrandRepository brandRepository, IDistributedCacheService distributedCacheService)
        {
            _mapper = mapper;
            _workContext = workContext;
            _brandRepository = brandRepository;
            _distributedCacheService = distributedCacheService;
        }
        public async Task<BrandView> CreateAsync(CreateBrandModel model, CancellationToken cancellationToken = default)
        {
            var brand = _mapper.Map<Brand>(model);
            brand.SeName = await GetSeName(brand);
            brand.Stores.Add(model.StoreId);
            brand.CreatedBy = _workContext.GetCurrentUserId();
            brand.CreatedOn = DateTime.UtcNow;
            var brandCreated = await _brandRepository.InsertAsync(brand);
            await DeleteAllCacheAsync(model.StoreId);
            return _mapper.Map<BrandView>(brandCreated);
        }

        public async Task<BrandView> UpdateAsync(UpdateBrandModel model, CancellationToken cancellationToken = default)
        {
            var brand = await GetByIdAsync(model.Id);

            if (brand.Name != model.Name)
                brand.SeName = await GetSeName(brand);

            brand.Name = model.Name;
            brand.Description = model.Description;
            brand.Icon = model.Icon;
            brand.PriceRanges = model.PriceRanges;
            brand.ShowOnHomePage = model.ShowOnHomePage;
            brand.FeaturedProductsOnHomePage = model.FeaturedProductsOnHomePage;
            brand.ShowOnSearchBox = model.ShowOnSearchBox;
            brand.SearchBoxDisplayOrder = model.SearchBoxDisplayOrder;
            brand.IncludeInTopMenu = model.IncludeInTopMenu;
            brand.SubjectToAcl = model.SubjectToAcl;
            brand.LimitedToStores = model.LimitedToStores;
            brand.Published = model.Published;
            brand.DisplayOrder = model.DisplayOrder;
            brand.MetaKeywords = model.MetaKeywords;
            brand.MetaDescription = model.MetaDescription;
            brand.MetaTitle = model.MetaTitle;
            brand.UpdatedOn = DateTime.UtcNow;
            brand.UpdatedBy = _workContext.GetCurrentUserId();
            var brandUpdated = await _brandRepository.UpdateAsync(brand);
            foreach (var store in brand.Stores)
                await DeleteAllCacheAsync(store);
            return _mapper.Map<BrandView>(brandUpdated);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(id);
            foreach(var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _brandRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(ids[0]);
            foreach (var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _brandRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<BrandView>> FindAsync(BrandQueryModel model, CancellationToken cancellationToken = default)
        {
            var brands = await FindAsync(model);
            return new PageableView<BrandView>
                (
                    model.PageIndex,
                    model.PageSize,
                    brands.Total,
                    _mapper.Map<IEnumerable<BrandView>>(brands.Data).ToList()
                );
        }

        public async Task<IEnumerable<BrandView>> GetAllByStoreIdAsync(string storeId, CancellationToken cancellationToken = default)
            =>  await _distributedCacheService.GetOrCreateAsync<IEnumerable<BrandView>>(GetAllBrandsCacheKey(storeId), async () =>
                {
                    var brands = await _brandRepository.FindAsync(x => x.Stores.Contains(storeId));
                    return _mapper.Map<IEnumerable<BrandView>>(brands);
                });

        public async Task<BrandView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<BrandView>(await GetByIdAsync(id));

        public async Task<BrandView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => _mapper.Map<BrandView>(await _brandRepository.FindOneAsync(x => x.SeName == slug));

        public async Task<BrandView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var brand = await GetByIdAsync(id);
            brand.Image = _mapper.Map<Image>(model);
            return _mapper.Map<BrandView>(await _brandRepository.UpdateAsync(brand));
        }

        public async Task<BrandView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var brand = await GetByIdAsync(id);
            brand.Image = null;
            return _mapper.Map<BrandView>(await _brandRepository.UpdateAsync(brand));
        }

        #region Private Methods
        private async Task<DataFilterPagingResult<Brand>> FindAsync(BrandQueryModel model)
        {
            Expression<Func<Brand, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText) ||
                x.MetaDescription.ToLower().Contains(searchText) ||
                x.MetaKeywords.ToLower().Contains(searchText) ||
                x.MetaTitle.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);

            var query = new DataFilterPaging<Brand> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };
            else
                query.Sort = new List<SortOption> { new SortOption { Field = nameof(Product.CreatedOn), Ascending = false } };
            return await _brandRepository.CountAndQueryAsync(query);
        }
        private async Task<Brand> GetByIdAsync(string id)
            => await _brandRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task<string> GetSeName(Brand brand)
        {
            if (!string.IsNullOrEmpty(brand.SeName) && !await _brandRepository.AnyAsync(x => x.SeName == brand.SeName))
                return brand.SeName;

            brand.SeName = SlugHelper.GenerateSlug(brand.Name);

            int i = 2;
            var tempSeName = brand.SeName;
            while (true)
            {
                if (await _brandRepository.AnyAsync(x => x.SeName == tempSeName))
                {
                    tempSeName = string.Format("{0}-{1}", brand.SeName, i);
                    i++;
                }
                else
                {
                    break;
                }

            }
            return tempSeName;
        }

        private async Task DeleteAllCacheAsync(string storeId)
        {
            await _distributedCacheService.RemoveAsync(GetAllBrandsCacheKey(storeId));
        }

        private string GetAllBrandsCacheKey(string storeId)
            => $"timecommerce.brand.getallwithparent.{storeId}";
        #endregion
    }
}
