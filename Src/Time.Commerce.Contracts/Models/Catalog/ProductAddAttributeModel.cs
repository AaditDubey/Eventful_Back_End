namespace Time.Commerce.Contracts.Models.Catalog
{
    public class ProductAddAttributeModel
    {
        public string AttributeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public int DisplayOrder { get; set; }
        public List<string> Values { get; set; }
    }
}
