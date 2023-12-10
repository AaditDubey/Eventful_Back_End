using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Catalog
{
    /// <summary>
    /// Represents a product category mapping
    /// </summary>
    public partial class ProductAttributeMapping : SubBaseEntity
    {
        public string AttributeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public int DisplayOrder { get; set; }
        public List<string> Values { get; set; }
    }
}
