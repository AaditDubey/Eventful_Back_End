using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface IDiscountCouponService
    {
        Task<DiscountCouponView> CreateAsync(CreateDiscountCouponModel model, CancellationToken cancellationToken = default);
        Task<DiscountCouponView> UpdateAsync(UpdateDiscountCouponModel model, CancellationToken cancellationToken = default);
        Task<DiscountCouponView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageableView<DiscountCouponView>> FindAsync(DiscountCouponQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
