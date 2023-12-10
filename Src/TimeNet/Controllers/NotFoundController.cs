using Microsoft.AspNetCore.Mvc;

namespace TimeNet.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
