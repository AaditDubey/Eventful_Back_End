namespace Time.Commerce.Contracts.Views.Catalog
{
    public partial interface ICategoryNode
    {
        string Id { get; set; }
        string Name { get; set; }
        string SeName { get; set; }
        string ImageUrl { get; set; }
        string ParentId { get; set; }
    }
}
