using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Identity;

public class SpeakerView
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Information { get; set; }
    public string ShortInformation { get; set; }
    public int DisplayOrder { get; set; }
    public bool Published { get; set; }
    public string SeName { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public ImageView Image { get; set; }
    public IList<CustomAttributeView> GenericAttributes { get; set; } = new List<CustomAttributeView>();
}
