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
    public class DiscountCouponService : IDiscountCouponService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IDiscountCouponRepository _discountCouponRepository;
        private readonly IDistributedCacheService _distributedCacheService;
        public DiscountCouponService(IMapper mapper, IWorkContext workContext, IDiscountCouponRepository discountCouponRepository, IDistributedCacheService distributedCacheService)
        {
            _mapper = mapper;
            _workContext = workContext;
            _discountCouponRepository = discountCouponRepository;
            _distributedCacheService = distributedCacheService;
        }
        public async Task<DiscountCouponView> CreateAsync(CreateDiscountCouponModel model, CancellationToken cancellationToken = default)
        {
            var discountCoupon = _mapper.Map<DiscountCoupon>(model);
            discountCoupon.Stores.Add(model.StoreId);
            discountCoupon.CreatedBy = _workContext.GetCurrentUserId();
            discountCoupon.CreatedOn = DateTime.UtcNow;
            var discountCouponCreated = await _discountCouponRepository.InsertAsync(discountCoupon);
            await DeleteAllCacheAsync(model.StoreId);
            return _mapper.Map<DiscountCouponView>(discountCouponCreated);
        }

        public async Task<DiscountCouponView> UpdateAsync(UpdateDiscountCouponModel model, CancellationToken cancellationToken = default)
        {
            var discountCoupon = await GetByIdAsync(model.Id);

            discountCoupon.CouponCode = model.CouponCode;
            discountCoupon.Used = model.Used;
            discountCoupon.Type = (DiscountType)model.Type;
            discountCoupon.UsePercentage = model.UsePercentage;
            discountCoupon.DiscountPercentage = model.DiscountPercentage;
            discountCoupon.DiscountAmount = model.DiscountAmount;
            discountCoupon.Description = model.Description;
            discountCoupon.Qty = model.Qty;
            discountCoupon.Published = model.Published;
            discountCoupon.LimitationTimes = model.LimitationTimes;
            discountCoupon.MaximumDiscountedQuantity = model.MaximumDiscountedQuantity;
            discountCoupon.MaximumDiscountAmount = model.MaximumDiscountAmount;
            discountCoupon.StartDateUtc = model.StartDateUtc;
            discountCoupon.EndDateUtc = model.EndDateUtc;
            discountCoupon.IdsApply = model.IdsApply;
            discountCoupon.UpdatedOn = DateTime.UtcNow;
            discountCoupon.UpdatedBy = _workContext.GetCurrentUserId();
            var discountCouponUpdated = await _discountCouponRepository.UpdateAsync(discountCoupon);
            foreach (var store in discountCoupon.Stores)
                await DeleteAllCacheAsync(store);
            return _mapper.Map<DiscountCouponView>(discountCouponUpdated);
        }


        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(id);
            foreach(var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _discountCouponRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(ids[0]);
            foreach (var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _discountCouponRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<DiscountCouponView>> FindAsync(DiscountCouponQueryModel model, CancellationToken cancellationToken = default)
        {
            var discountCoupons = await GetAllAsync(model);
            return new PageableView<DiscountCouponView>
                (
                    model.PageIndex,
                    model.PageSize,
                    discountCoupons.Total,
                    _mapper.Map<IEnumerable<DiscountCouponView>>(discountCoupons.Data).ToList()
                );
        }

        public async Task<DiscountCouponView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<DiscountCouponView>(await GetByIdAsync(id));

        #region Private Methods
        private async Task<DataFilterPagingResult<DiscountCoupon>> GetAllAsync(DiscountCouponQueryModel model)
        {
            Expression<Func<DiscountCoupon, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.CouponCode.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (model.CouponCodes is not null && model.CouponCodes.Any())
                filter = filter.And(x => x.CouponCode != null && model.CouponCodes.Contains(x.CouponCode));

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);

            var query = new DataFilterPaging<DiscountCoupon> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _discountCouponRepository.CountAndQueryAsync(query);
        }
        private async Task<DiscountCoupon> GetByIdAsync(string id)
            => await _discountCouponRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task DeleteAllCacheAsync(string storeId)
        {
            await _distributedCacheService.RemoveAsync(GetCacheKey(DiscountCouponCacheEnum.GetAllWithParent, storeId));
            await _distributedCacheService.RemoveAsync(GetCacheKey(DiscountCouponCacheEnum.GetAllWithRecursive, storeId));
        }

        private string GetCacheKey(DiscountCouponCacheEnum type, string storeId)
            => type == DiscountCouponCacheEnum.GetAllWithParent ? $"timecommerce.discountCoupon.getallwithparent.{storeId}" : $"timecommerce.discountCoupon.getallwithrecursive.{storeId}";
        #endregion

        private enum DiscountCouponCacheEnum
        {
            GetAllWithParent,
            GetAllWithRecursive
        }
    }
}
