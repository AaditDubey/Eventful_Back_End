namespace Time.Commerce.Contracts.Views.Catalog
{
    public partial class CategoryRecursiveView
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string SeName { get; set; }
        public List<CategoryRecursiveView> Children { get; set; }
    }
}
