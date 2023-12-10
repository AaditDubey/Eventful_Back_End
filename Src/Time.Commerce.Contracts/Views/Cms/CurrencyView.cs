namespace Time.Commerce.Contracts.Views.Cms
{
    public class CurrencyView
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsLocalBank { get; set; }
        public string CountryCode { get; set; }
    }
}
