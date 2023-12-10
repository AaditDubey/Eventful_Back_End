namespace Time.Commerce.Domains.Entities.Catalog
{
    public class ProductOverView : Product
    {
        public Brand Brand { get; set; }
        public IList<Category> Categories { get; set; }
    }
}
