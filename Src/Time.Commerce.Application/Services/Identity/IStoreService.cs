using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IStoreService
    {
        Task<StoreView> CreateAsync(CreateStoreModel model, CancellationToken cancellationToken = default);
        Task<StoreView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<StoreView> GetByStoreIdAsync(string storeId, CancellationToken cancellationToken = default);
        Task<StoreDetailsView> GetStoreDetailsAsync(string? storeId, CancellationToken cancellationToken = default);
        Task<DataFilterPagingResult<Store>> GetListAsync(DataFilterPaging<Store> model, CancellationToken cancellationToken = default);
        Task<PageableView<StoreView>> FindAsync(StoreQueryModel model, CancellationToken cancellationToken = default);
        Task<StoreView> UpdateAsync(UpdateStoreModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<StoreView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<StoreView> DeleteImage(string id, CancellationToken cancellationToken = default);
        Task<bool> CheckStoreExistsAsync(string store, CancellationToken cancellationToken = default);
        Task<bool> InstallStoreAsync(RegisterStoreModel model, CancellationToken cancellationToken = default);
        Task<bool> InstallThemeAsync(string key, CancellationToken cancellationToken = default);
    }
}
