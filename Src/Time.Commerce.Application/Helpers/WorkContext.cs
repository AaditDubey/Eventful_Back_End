using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Time.Commerce.Application.Helpers
{
    public interface IWorkContext
    {
        string GetCurrentUserId();
        string GetCurrentUserEmail();
        string GetCurrentStoreId();
        string GetCurrentIpAddress();
        string GetCurrentRole();

        UserClaimsInfo? GetUserClaimsInfo();
    }
    public class WorkContext: IWorkContext
    {
        private IHttpContextAccessor _httpContextAccessor;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId()
            => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

        public string GetCurrentUserEmail()
            => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "";

        public string GetCurrentStoreId()
           => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "store")?.Value ?? "";

        public string GetCurrentIpAddress()
            => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        public UserClaimsInfo? GetUserClaimsInfo()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            return new UserClaimsInfo
            {
                Id = GetCurrentUserId(),
                Email = GetCurrentUserEmail(),
            };
        }

        public string GetCurrentRole()
          => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? "";
    }

    public class UserClaimsInfo
    {
        public string Id { get; set;}
        public string Email { get; set; }
    }
}
