using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class CreateBrandModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string PriceRanges { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool FeaturedProductsOnHomePage { get; set; }
        public bool ShowOnSearchBox { get; set; }
        public int SearchBoxDisplayOrder { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public bool SubjectToAcl { get; set; }
        public bool LimitedToStores { get; set; }
        public string SeName { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string StoreId { get; set; }
        public ImageModel Image { get; set; }
    }
}
