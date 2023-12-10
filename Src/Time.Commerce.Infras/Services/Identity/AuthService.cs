using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Options;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;

namespace Time.Commerce.Infras.Services.Identity
{
    public class AuthService : IAuthService
    {
        #region Fieds
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthOptions _authOptions;
        #endregion

        #region Ctor
        public AuthService(IMapper mapper, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor; 
            _authOptions = _configuration.GetSection("AuthOptions").Get<AuthOptions>();
        }
        #endregion

        #region Methods
        public async Task<LoginView> SignIn(LoginModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindOneAsync(x => x.UserName == model.UserName && x.Active) ??
                     throw new BadRequestException(nameof(CommonErrors.LOGIN_INVALID), CommonErrors.LOGIN_INVALID);

            if (!SecurityHelpers.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                throw new BadRequestException(nameof(CommonErrors.LOGIN_INVALID), CommonErrors.LOGIN_INVALID);

            var userView = _mapper.Map<UserView>(user);
            var (accessToken, exp) = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await StoredRefreshToken(user.Id, refreshToken);

            return new LoginView
            {
                User = userView,
                AccessToken = accessToken,
                Exp = exp,
                RefreshToken = refreshToken
            };
        }
        public async Task<TokenView> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var oldRefreshToken = await _refreshTokenRepository.FindOneAsync(x => x.Token == refreshToken && x.RevokedDate == null && x.IpAddress == _remoteIpAddress && string.IsNullOrEmpty(x.ReplacedByToken) && x.ExpiresDate >= DateTimeOffset.UtcNow);
            var user = await _userRepository.GetByIdAsync(oldRefreshToken.UserId.ToString());

            var (accessToken, exp) = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await StoredRefreshToken(user.Id, newRefreshToken);

            //update old refresh token
            oldRefreshToken.RevokedDate = DateTimeOffset.UtcNow;
            oldRefreshToken.ReplacedByToken = newRefreshToken;
            await _refreshTokenRepository.UpdateAsync(oldRefreshToken);
            return new TokenView
            {
                AccessToken = accessToken,
                Exp = exp,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> SignInWithCookies(LoginCookiesModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindOneAsync(x => x.UserName == model.UserName && x.Active);
            if(user == null)
                return false;

            if (!SecurityHelpers.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                return false;
            var claims = GenerateClaims(user);

            if (claims == null)
                return false;

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTime.UtcNow.AddMonths(1),
                AllowRefresh = true
            };
            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return true;
        }

        public async Task<bool> SignOutWithCookies(CancellationToken cancellationToken = default)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }
        #endregion

        #region Private Methods
        private IEnumerable<Claim> GenerateClaims(User user)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _authOptions.Iss));
            claims.Add(new Claim(ClaimTypes.Email, user.UserName));
            //claims.Add(new Claim("store", user.UserStoreMapping.FirstOrDefault()?.StoreUrl));

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

            return claims;
        }

        private (string, decimal) GenerateAccessToken(User user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authOptions.Secret));
            var timeSpan = TimeSpan.FromMinutes(_authOptions.TokenExpiresTimeMinutesIn);
            var now = DateTime.UtcNow;
            var expires_date = now.Add(timeSpan);
            var claims = GenerateClaims(user);

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Iss,
                audience: _authOptions.Aud,
                claims: claims,
                notBefore: now,
                expires: expires_date,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (accessToken, (decimal)Math.Floor(expires_date.Subtract(DateTime.UnixEpoch).TotalSeconds));
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task StoredRefreshToken(string userId, string token)
        {
            var refreshToken = new RefreshToken { UserId = userId, Token = token, IpAddress = _remoteIpAddress, ExpiresDate = DateTimeOffset.UtcNow.AddMinutes(_authOptions.RefreshTokenExpiresTimeMinutesIn) };
            await _refreshTokenRepository.InsertAsync(refreshToken);
        }

        private string _remoteIpAddress => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        #endregion
    }
}
