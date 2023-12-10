using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Core.Helpers;

namespace TimeNet.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IAccountService _accountService;
    private readonly IStoreService _storeService;
    private readonly IWorkContext _workContext;
    public AccountController(IAuthService authService, IAccountService accountService, IStoreService storeService, IWorkContext workContext)
    {
        _authService = authService;
        _accountService = accountService;
        _storeService = storeService;
        _workContext = workContext;
    }
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Signup()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ForgotPassword(string email)
    {
        return Ok(true);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCookiesModel model, string returnUrl = "/", bool createPersistentCookie = false)
    {
        model.RememberMe = createPersistentCookie;
        var res = await _authService.SignInWithCookies(model);
        if (!res)
        {
            ViewBag.Error = "Username or password in correct.";
            return View();
        }

        if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/";

        return Redirect(returnUrl);
    }


    [HttpPost]
    public async Task<IActionResult> LoginApp([FromBody] LoginCookiesModel model, string returnUrl = "/", bool createPersistentCookie = false)
    {
        model.RememberMe = createPersistentCookie;
        var res = await _authService.SignInWithCookies(model);
        if (!res)
        {
            ViewBag.Error = "Username or password in correct.";
            return View();
        }

        if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/";

        return Redirect(returnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> Signup(RegisterModel model, string returnUrl = "", bool createPersistentCookie = false, CancellationToken cancellationToken = default)
    {
        var existingEmail = await _accountService.CheckEmailExist(model.Email, cancellationToken);
        if(existingEmail)
        {
            ViewBag.Error = "Email address already exits.";
            return View();
        }

        var account = await _accountService.Register(model, cancellationToken);
        if(account is null)
        {
            ViewBag.Error = "Some things went wrongs. Try again please!";
            return View();
        }
     
        return await Login(new LoginCookiesModel { UserName = account.Email, Password = model.Password });
    }

    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutWithCookies();
        return Redirect("/");
    }

    [HttpPost()]
    public async Task<IActionResult> CheckStoreExists(string storeId, CancellationToken cancellationToken)
        => Ok(await _storeService.CheckStoreExistsAsync(storeId, cancellationToken));

    [HttpPost()]
    public async Task<IActionResult> CheckEmailExist(string email, CancellationToken cancellationToken)
        => Ok(await _accountService.CheckEmailExist(email, cancellationToken));
}