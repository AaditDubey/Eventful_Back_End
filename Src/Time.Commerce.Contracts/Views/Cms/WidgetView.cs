using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Cms;

public class WidgetView
{
    public string Id { get; set; }
    public string StoreId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Html { get; set; }
    public string Zone { get; set; }
    public int DisplayOrder { get; set; }
    public int NumberOfProduct { get; set; }
    public string ItemId { get; set; }
    public IList<WidgetCarouselView>? WidgetCarousels { get; set; }
    public WidgetFootersView? WidgetFooters { get; set; }
    public IList<WidgetMenusView>? WidgetMenus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

public class WidgetCarouselView
{
    public string Id { get; set; }
    public string Caption { get; set; }
    public string SubCaption { get; set; }
    public string LinkUrl { get; set; }
    public string LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public ImageView Image { get; set; }
}
public class WidgetMenusView : IMenuNode
{
    public string Id { get; set; }
    public string MenuId { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public int DisplayOrder { get; set; }
}
public class WidgetFootersView
{
    public string Id { get; set; }
    public int TotalColumns { get; set; }
    public IList<WidgetFooterColumnView> Columns { get; set; }

}
public class WidgetFooterColumnView
{
    public string Id { get; set; }
    public int DisplayOrder { get; set; }
    public string Title { get; set; }
    public List<WidgetFootersRowView> WidgetFootersRows { get; set; }
}
public class WidgetFootersRowView
{
    public string Id { get; set; }
    public string LinkUrl { get; set; }
    public string LinkText { get; set; }
    public int DisplayOrder { get; set; }
}

