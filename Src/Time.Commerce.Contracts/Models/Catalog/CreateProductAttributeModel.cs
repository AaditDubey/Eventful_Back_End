namespace Time.Commerce.Contracts.Models.Catalog
{
    public class CreateProductAttributeModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Published { get; set; }
        public bool IsColorAttribute { get; set; }
        public string StoreId { get; set; }
        public List<ProductAttributeOptionValueModel> OptionValues { get; set; }
    }
}
