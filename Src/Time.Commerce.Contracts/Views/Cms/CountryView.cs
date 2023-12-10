namespace Time.Commerce.Contracts.Views.Cms
{
    public class CountryView
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhonePrefix { get; set; }
        public bool Enabled { get; set; }
    }
}
