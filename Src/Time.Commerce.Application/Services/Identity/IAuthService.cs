using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IAuthService
    {
        Task<LoginView> SignIn(LoginModel model, CancellationToken cancellationToken = default);
        Task<TokenView> RefreshToken(string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> SignInWithCookies(LoginCookiesModel model, CancellationToken cancellationToken = default);
        Task<bool> SignOutWithCookies(CancellationToken cancellationToken = default);
    }
}
