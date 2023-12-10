using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Cms;

public class Widget: BaseAuditEntity
{
    public string StoreId { get; set; }
    public WidgetTypes Type { get; set; }
    public string Name { get; set; }
    public string Html { get; set; }
    public string Zone { get; set; }
    public int DisplayOrder { get; set; }
    public int NumberOfProduct { get; set; }
    public string ItemId { get; set; }
    public IList<WidgetCarousel>? WidgetCarousels { get; set; }
    public WidgetFooters? WidgetFooters { get; set; }
    public IList<WidgetMenus>? WidgetMenus { get; set; }
}

public class WidgetCarousel : SubBaseEntity
{
    public WidgetCarousel() => Image = new Image();
    public string Caption { get; set; }
    public string SubCaption { get; set; }
    public string LinkUrl { get; set; }
    public string LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public Image Image { get; set; }
}
public class WidgetMenus: SubBaseEntity
{
    public string MenuId { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public int DisplayOrder { get; set; }
}
public class WidgetFooters : SubBaseEntity
{
    public int TotalColumns { get; set; }
    public IList<WidgetFooterColumn> Columns { get; set; }

}
public class WidgetFooterColumn : SubBaseEntity
{
    public int DisplayOrder { get; set; }
    public string Title { get; set; }
    public List<WidgetFootersRow> WidgetFootersRows { get; set; }
}
public class WidgetFootersRow : SubBaseEntity
{
    public string LinkUrl { get; set; }
    public string LinkText { get; set; }
    public int DisplayOrder { get; set; }
}
