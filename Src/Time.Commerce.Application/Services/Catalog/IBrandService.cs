using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface IBrandService
    {
        Task<BrandView> CreateAsync(CreateBrandModel model, CancellationToken cancellationToken = default);
        Task<BrandView> UpdateAsync(UpdateBrandModel model, CancellationToken cancellationToken = default);
        Task<BrandView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<BrandView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<PageableView<BrandView>> FindAsync(BrandQueryModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<BrandView>> GetAllByStoreIdAsync(string storeId, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<BrandView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<BrandView> DeleteImage(string id, CancellationToken cancellationToken = default);
    }
}
