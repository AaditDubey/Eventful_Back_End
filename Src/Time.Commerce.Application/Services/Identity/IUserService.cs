using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IUserService
    {
        Task<UserView> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken = default);
        Task<UserView> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<DataFilterPagingResult<User>> GetListAsync(DataFilterPaging<User> model, CancellationToken cancellationToken = default);
        Task<PageableView<UserView>> FindAsync(UserQueryModel model, CancellationToken cancellationToken = default);
        Task<UserView> UpdateUserAsync(UpdateUserModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<UserView> AddStoreAsync(User user, string storeId, CancellationToken cancellationToken = default);
    }
}
