using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Catalog
{
    public class Brand : BaseAuditEntity
    {
        #region Ctor
        public Brand()
        {
            Stores = new List<string>();
            Locales = new List<LocalizedProperty>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the parent category identifier
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the image
        /// </summary>
        public Image? Image { get; set; }

        /// <summary>
        /// Gets or sets the available price ranges
        /// </summary>
        public string PriceRanges { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the category on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show featured products on home page
        /// </summary>
        public bool FeaturedProductsOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the category on search box
        /// </summary>
        public bool ShowOnSearchBox { get; set; }

        /// <summary>
        /// Gets or sets the display order on search box category
        /// </summary>
        public int SearchBoxDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include this category in the top menu
        /// </summary>
        public bool IncludeInTopMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }
        public IList<string> Stores { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string SeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the Icon
        /// </summary>
        public string Icon { get; set; }
        public IList<LocalizedProperty> Locales { get; set; }
        #endregion

        #region SEO Properties
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }
        #endregion

        #region Navigation Properties
        private ICollection<string> _appliedDiscounts;

        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        public virtual ICollection<string> AppliedDiscounts
        {
            get { return _appliedDiscounts ??= new List<string>(); }
            protected set { _appliedDiscounts = value; }
        }
        #endregion
    }
}
