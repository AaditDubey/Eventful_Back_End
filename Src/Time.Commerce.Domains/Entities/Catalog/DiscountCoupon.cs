using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Catalog
{
    public class DiscountCoupon : BaseAuditEntity
    {
        public DiscountCoupon()
        {
            Stores = new List<string>();
            IdsApply = new List<string>();
        }

        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets discount used
        /// </summary>
        public bool Used { get; set; }
        /// <summary>
        /// Gets or sets the discount type identifier
        /// </summary>
        public DiscountType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// How many times was used
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the discount limitation times (used when Limitation is set to "N Times Only" or "N Times Per Customer")
        /// </summary>
        public int LimitationTimes { get; set; }

        /// <summary>
        /// Gets or sets the maximum product quantity which could be discounted
        /// Used with "Assigned to products" or "Assigned to categories" type
        /// </summary>
        public int MaximumDiscountedQuantity { get; set; }
        public decimal MaximumDiscountAmount { get; set; }
        /// <summary>
        /// Gets or sets the discount start date and time
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the discount end date and time
        /// </summary>
        public DateTime? EndDateUtc { get; set; }
        public IList<string> Stores { get; set; }
        public IList<string> IdsApply { get; set; }
    }
}
