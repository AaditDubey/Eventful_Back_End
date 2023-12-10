using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Application.Services.Cms
{
    public interface ILocaleService
    {
        Task<IEnumerable<CountryView>> GetCountriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<StateProvinceView>> GetStateProvincesAsync(string countryCode = "", CancellationToken cancellationToken = default);
        Task<IEnumerable<CurrencyView>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    }
}
