namespace Time.Commerce.Contracts.Views.Catalog
{
    public class ProductAttributeView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Published { get; set; }
        public bool IsColorAttribute { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IList<string> Stores { get; set; }
        public IList<ProductAttributeOptionValueView> OptionValues { get; set; }
    }
}
