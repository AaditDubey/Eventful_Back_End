using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Sales
{
    /// <summary>
    /// Represents an order item
    /// </summary>
    public partial class OrderItem : SubBaseEntity
    {
        #region Product
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureThumbnailUrl { get; set; }
        /// <summary>
        /// Gets or sets the sku product identifier
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// Gets or sets the attribute description
        /// </summary>
        public string AttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the product attributes
        /// </summary>
        public IList<CustomAttribute> Attributes { get; set; } = new List<CustomAttribute>();
        /// <summary>
        /// Gets or sets the total weight of one item
        /// </summary>       
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal? ItemWeight { get; set; }
        #endregion

        #region Vendor
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the warehouse identifier
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price without discount in primary store currency (incl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal UnitPriceWithoutDiscInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price without discount in primary store currency (excl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal UnitPriceWithoutDiscExclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (incl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (excl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (incl tax)
        /// </summary>
        public decimal PriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (excl tax)
        /// </summary>
        public decimal PriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (incl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal DiscountAmountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (excl tax)
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal DiscountAmountExclTax { get; set; }

        /// <summary>
        /// Gets or sets the original cost of this order item (when an order was placed), qty 1
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal OriginalProductCost { get; set; }

        /// <summary>
        /// Gets or sets the rental product start date (null if it's not a rental product)
        /// </summary>
        public DateTime? RentalStartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the rental product end date (null if it's not a rental product)
        /// </summary>
        public DateTime? RentalEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the vendor`s commission
        /// </summary>
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Commission { get; set; }

    }
}
