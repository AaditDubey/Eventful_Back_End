namespace Time.Commerce.Contracts.Views.Catalog
{
    public class ProductOverViewView : ProductView
    {
        public BrandView Brand { get; set; }
        public IList<CategoryView> Categories { get; set; }
    }
}
