using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Identity;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class ThemesController : BaseApiController
{
    private readonly IStoreService _storeService;
    public ThemesController(IStoreService storeService)
        => _storeService = storeService;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        return Ok(await _storeService.GetStoreDetailsAsync(default));
    }


    [HttpGet("install")]
    public async Task<IActionResult> Install(string key, CancellationToken cancellationToken = default)
    {
        return Ok(await _storeService.InstallThemeAsync(key, cancellationToken));
    }
}