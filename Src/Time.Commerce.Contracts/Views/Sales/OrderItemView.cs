using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Sales
{
    public partial class OrderItemView
    {
        #region Product
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public string PictureThumbnailUrl { get; set; }
        /// <summary>
        /// Gets or sets the attribute description
        /// </summary>
        public string AttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the product attributes
        /// </summary>
        public IList<CustomAttributeView> Attributes { get; set; } = new List<CustomAttributeView>();
        /// <summary>
        /// Gets or sets the total weight of one item
        /// </summary>       
        public decimal? ItemWeight { get; set; }
        #endregion

        #region Vendor
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        #endregion

        public decimal PriceInclTax { get; set; }
        public decimal PriceExclTax { get; set; }
        public decimal UnitPriceInclTax { get; set; }
        public decimal UnitPriceExclTax { get; set; }

        public int Quantity { get; set; }


        public string DiscountInclTax { get; set; }
        public string DiscountExclTax { get; set; }
        public decimal DiscountInclTaxValue { get; set; }
        public decimal DiscountExclTaxValue { get; set; }

        public string SubTotalInclTax { get; set; }
        public string SubTotalExclTax { get; set; }
        public decimal SubTotalInclTaxValue { get; set; }
        public decimal SubTotalExclTaxValue { get; set; }

        public string AttributeInfo { get; set; }
        public string RecurringInfo { get; set; }
        public string RentalInfo { get; set; }
        public IList<string> ReturnRequestIds { get; set; }
        public IList<string> PurchasedGiftCardIds { get; set; }
        public string Commission { get; set; }
        public decimal CommissionValue { get; set; }
    }
}
