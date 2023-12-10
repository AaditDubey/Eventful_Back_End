using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class VendorQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public bool? Active { get; set; }   
    }
}
