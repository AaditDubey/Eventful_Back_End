using Time.Commerce.Application.Extensions;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Infras.Services.Cms
{
    public class LocaleService : ILocaleService
    {
        private readonly IDistributedCacheService _distributedCacheService;
        public LocaleService(IDistributedCacheService distributedCacheService)
        {
            _distributedCacheService = distributedCacheService;
        }
        public async Task<IEnumerable<CountryView>> GetCountriesAsync(CancellationToken cancellationToken = default)
            =>  await _distributedCacheService.GetOrCreateAsync<IEnumerable<CountryView>>("get_countries_cache_key", async () =>
                {
                    var path = Path.Combine(Path.GetDirectoryName(typeof(LocaleService).Assembly.Location), @"Data/countries.csv");
                    var records = CsvExtension.GetRecordsForFile<CountryView>(path);
                    var listWithoutFlag = new string[] { "AQ", "BQ", "BV", "GF", "TF", "GP", "HM", "NC", "RE", "SH", "MF", "PM", "GS", "SJ", "UM", "WF" };
                    return records.Where(x => !listWithoutFlag.Contains(x.Code));
                });

        public async Task<IEnumerable<StateProvinceView>> GetStateProvincesAsync(string countryCode = "", CancellationToken cancellationToken = default)
            =>  await _distributedCacheService.GetOrCreateAsync<IEnumerable<StateProvinceView>>($"get_state_provinces_{countryCode}_cache_key", async () =>
                {
                    var path = Path.Combine(Path.GetDirectoryName(typeof(LocaleService).Assembly.Location), @"Data/cities.csv");
                    var records = CsvExtension.GetRecordsForFile<StateProvinceView>(path);
                    if(!string.IsNullOrEmpty(countryCode))
                        return records.Where(x => x.CountryCode == countryCode);
                    return records;
                });

        public async Task<IEnumerable<CurrencyView>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
            => await _distributedCacheService.GetOrCreateAsync<IEnumerable<CurrencyView>>($"get_currencies_cache_key", async () =>
            {
                var path = Path.Combine(Path.GetDirectoryName(typeof(LocaleService).Assembly.Location), @"Data/currencies.csv");
                return CsvExtension.GetRecordsForFile<CurrencyView>(path);
            });
    }
}
