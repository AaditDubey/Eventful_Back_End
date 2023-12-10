using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Cms;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class LocaleController : BaseApiController
{
    private readonly ILocaleService _localeService;
    public LocaleController(ILocaleService localeService)
        => _localeService = localeService;

    [Route("getCountries")]
    [HttpGet]
    public async Task<IActionResult> GetCountries(CancellationToken cancellationToken = default)
        => Ok(await _localeService.GetCountriesAsync(cancellationToken));

    [Route("getStateProvinces")]
    [HttpGet]
    public async Task<IActionResult> GetStateProvinces(string countryCode = "", CancellationToken cancellationToken = default)
        => Ok(await _localeService.GetStateProvincesAsync(countryCode, cancellationToken));

    [Route("getCurrencies")]
    [HttpGet]
    public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken = default)
     => Ok(await _localeService.GetCurrenciesAsync(cancellationToken));
}