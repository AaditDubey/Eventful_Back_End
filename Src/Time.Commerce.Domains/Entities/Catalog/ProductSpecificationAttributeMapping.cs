using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Catalog
{
    public class ProductSpecificationAttributeMapping : SubBaseEntity
    {
        /// <summary>
        /// Gets or sets the attribute type ID. Get From SpecificationAttributeType enums
        /// </summary>
        public string AttributeType { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the custom value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets whether the attribute can be filtered by
        /// </summary>
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
