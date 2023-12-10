using Microsoft.AspNetCore.Mvc;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : BaseCookieEventHostAuthController
{
    public IActionResult Index()
    {
        ViewBag.AdminTest = "AdminTest";
        return View();
    }
}
