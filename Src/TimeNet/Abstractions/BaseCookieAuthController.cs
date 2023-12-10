using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Core.Constants;

namespace TimeNet.Abstractions;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = CoreSystemConst.SYSTEM_ADMIN_POLICY)]
public abstract class BaseCookieAuthController : Controller
{
}
