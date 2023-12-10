using Time.Commerce.Contracts.Enums.Cms;
using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Cms;

public class WidgetQueryModel : BaseQueryModel
{
    public string StoreId { get; set; }
    public WidgetTypes? Type { get; set; }
}
