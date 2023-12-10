using System.Collections.Generic;
using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class DiscountCouponQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public bool? Published { get; set; }
        public IList<string> CouponCodes { get; set; }
    }
}
