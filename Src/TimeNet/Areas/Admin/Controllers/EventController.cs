using Microsoft.AspNetCore.Mvc;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class EventController : BaseCookieEventHostAuthController
{
    #region Mvc Actions
    public IActionResult Index(string searchText)
    {
        ViewBag.SearchText = searchText;
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    public IActionResult Update(string id)
    {
        ViewBag.Id = id;
        return View();
    }

    public IActionResult AddByAi()
    {
        return View();
    }
    #endregion
}