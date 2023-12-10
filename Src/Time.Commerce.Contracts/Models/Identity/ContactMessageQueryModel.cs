using Time.Commerce.Contracts.Enums.Identity;
using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Identity;

public class ContactMessageQueryModel : BaseQueryModel
{
    public ContactMessageType? Type { get; set; }
}