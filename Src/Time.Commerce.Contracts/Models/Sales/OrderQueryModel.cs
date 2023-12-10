using Time.Commerce.Contracts.Enums.Sales;
using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Sales
{
    public class OrderQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public ShippingStatus? ShippingStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
