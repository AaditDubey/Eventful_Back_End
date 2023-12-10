using Microsoft.AspNetCore.Mvc;

namespace TimeNet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/admin");
        }
    }
}
