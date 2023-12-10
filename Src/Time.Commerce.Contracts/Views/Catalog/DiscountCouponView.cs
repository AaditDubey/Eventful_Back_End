using Time.Commerce.Contracts.Enums.Catalog;

namespace Time.Commerce.Contracts.Views.Catalog
{
    public class DiscountCouponView
    {
        public string Id { get; set; }
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool Used { get; set; }
        public DiscountType Type { get; set; }
        public bool UsePercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public int Qty { get; set; }
        public bool IsEnabled { get; set; }
        public int LimitationTimes { get; set; }
        public int MaximumDiscountedQuantity { get; set; }
        public decimal MaximumDiscountAmount { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public DateTime? EndDateUtc { get; set; }
        public IList<string> Stores { get; set; }
        public IList<string> IdsApply { get; set; }
    }
}
