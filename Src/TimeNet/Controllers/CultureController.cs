using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace TimeNet.Controllers;

public class CultureController : Controller
{
    public IActionResult SetCulture(string culture, string returnUrl)
    {
        //Set new culture cookie
        Response.Cookies.Append(
           CookieRequestCultureProvider.DefaultCookieName,
           CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
           new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
       );
        return LocalRedirect(returnUrl);
    }
}