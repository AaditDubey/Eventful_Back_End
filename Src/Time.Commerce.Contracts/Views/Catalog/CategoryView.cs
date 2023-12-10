using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Catalog
{
    public class CategoryView : ICategoryNode
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IList<string> Stores { get; set; }
        public ImageView Image { get; set; }

        /// <summary>
        /// This show name with parent. Ex: New >> Child
        /// </summary>
        public string LevelName { get; set; }
        public string ImageUrl { get; set; }
    }
}
