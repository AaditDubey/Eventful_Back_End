using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Cms
{
    public class CreateContentModel
    {
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string FullContent { get; set; }
        public IList<string> Tags { get; set; }
        public string Type { get; set; }
        public string Publisher { get; set; }
        public string SeName { get; set; }
        public bool Published { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string StoreId { get; set; }
        public ImageModel Image { get; set; }
    }
}
