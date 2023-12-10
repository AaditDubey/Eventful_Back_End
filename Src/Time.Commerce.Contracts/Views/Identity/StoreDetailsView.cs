using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Contracts.Views.Identity;

public class StoreDetailsView : StoreView
{
    public IList<WidgetView>? Widgets { get; set; }
    public IList<MenuView>? HeaderMenus { get; set; }
}