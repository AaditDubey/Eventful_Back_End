adminApp.service('LocaleService', function (CommonService) {
    this.GetCountries = () => CommonService.Http.Get(`/api/v1/locale/getCountries`)
    this.GetCurrencies = () => CommonService.Http.Get(`/api/v1/locale/getCurrencies`)
    this.GetStateProvinces = (countryCode) => CommonService.Http.Get(`/api/v1/locale/getStateProvinces?countryCode=${countryCode}`)
})