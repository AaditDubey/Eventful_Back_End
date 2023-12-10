using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IVendorService
    {
        Task<VendorView> CreateVendorAsync(CreateVendorModel model, CancellationToken cancellationToken = default);
        Task<VendorView> GetVendorByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<DataFilterPagingResult<Vendor>> GetListAsync(DataFilterPaging<Vendor> model, CancellationToken cancellationToken = default);
        Task<PageableView<VendorView>> FindAsync(VendorQueryModel model, CancellationToken cancellationToken = default);
        Task<VendorView> UpdateVendorAsync(UpdateVendorModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteVendorAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<VendorView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<VendorView> DeleteImage(string id, CancellationToken cancellationToken = default);
    }
}
