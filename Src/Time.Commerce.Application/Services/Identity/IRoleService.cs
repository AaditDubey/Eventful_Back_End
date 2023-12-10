using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IRoleService
    {
        Task<RoleView> CreateRoleAsync(CreateRoleModel model, CancellationToken cancellationToken = default);
        Task<RoleView> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<DataFilterPagingResult<Role>> GetListAsync(DataFilterPaging<Role> model, CancellationToken cancellationToken = default);
        Task<PageableView<RoleView>> FindAsync(RoleQueryModel model, CancellationToken cancellationToken = default);
        Task<RoleView> UpdateRoleAsync(UpdateRoleModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteRoleAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
