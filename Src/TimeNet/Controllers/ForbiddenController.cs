using Microsoft.AspNetCore.Mvc;

namespace TimeNet.Controllers
{
    public class ForbiddenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
