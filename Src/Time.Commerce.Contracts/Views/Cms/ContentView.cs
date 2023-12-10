using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Cms
{
    public class ContentView
    {
        public string Id { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IList<string> Stores { get; set; }
        public ImageView Image { get; set; }
    }
}
