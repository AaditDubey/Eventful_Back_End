using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Cms
{
    public class Content : BaseAuditEntity
    {
        #region Ctor
        public Content()
        {
            Stores = new List<string>();
            Tags = new List<string>();
            Locales = new List<LocalizedProperty>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string FullContent { get; set; }
        public IList<string> Tags { get; set; }
        public Image Image { get; set; }

        /// <summary>
        /// GEt or set type. To separate is page or news when store together DB.
        /// </summary>
        public string Type { get; set; }
        public string Publisher { get; set; }
        public string SeName { get; set; }
        public bool Published { get; set; }
        public IList<string> Stores { get; set; }
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
    }
}
