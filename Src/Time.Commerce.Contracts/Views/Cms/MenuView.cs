namespace Time.Commerce.Contracts.Views.Cms;

public partial class MenuView
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public List<MenuView> Children { get; set; }
}

public partial interface IMenuNode
{
    public string Id { get; set; }
    public string MenuId { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public int DisplayOrder { get; set; }
}
