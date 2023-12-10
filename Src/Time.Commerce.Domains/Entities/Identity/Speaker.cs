using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Identity;

public class Speaker : BaseAuditEntity
{
    #region Ctor
    public Speaker()
    {
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
    /// Gets or sets the Information
    /// </summary>
    public string Information { get; set; }
    /// <summary>
    /// Gets or sets the short Information
    /// </summary>
    public string ShortInformation { get; set; }

    /// <summary>
    /// Gets or sets the image
    /// </summary>
    public Image? Image { get; set; }

    public string SeName { get; set; }
    public bool ShowOnHomePage { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
   
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
  
    #endregion
}
