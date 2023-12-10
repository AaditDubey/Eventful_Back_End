using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IAccountService
    {
        Task<UserView> Register(RegisterModel model, CancellationToken cancellationToken = default);
        Task<UserView> RegisterStore(RegisterStoreModel model, CancellationToken cancellationToken = default);
        Task<bool> CheckEmailExist(string email, CancellationToken cancellationToken = default);
        Task<bool> ChangePassword(ChangePasswordModel model, CancellationToken cancellationToken = default);
    }
}
