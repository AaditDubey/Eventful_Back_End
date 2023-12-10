namespace Time.Commerce.Contracts.Views.Catalog
{
    public class ProductAttributeMappingView
    {
        public string Id { get; set; }
        public string AttributeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public int DisplayOrder { get; set; }
        public List<string> Values { get; set; }
    }
}
