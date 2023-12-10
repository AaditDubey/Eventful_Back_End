using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Catalog
{
    public class ProductAttribute : BaseAuditEntity
    {
        #region Ctor
        public ProductAttribute()
        {
            Stores = new List<string>();
            OptionValues = new List<ProductAttributeOptionValue>();
            Locales = new List<LocalizedProperty>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool IsColorAttribute { get; set; }
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Type { get; set; }
        public IList<string> Stores { get; set; }
        public IList<ProductAttributeOptionValue> OptionValues { get; set; }
        public IList<LocalizedProperty> Locales { get; set; }
        #endregion
    }
}
