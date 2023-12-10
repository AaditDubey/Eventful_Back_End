using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.Contracts.Views.Catalog
{
    public class ProductView
    {
        public string Id { get; set; }
        #region Properties
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sename
        /// </summary>
        public string SeName { get; set; }
        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }
      
        /// <summary>
        /// Gets or sets a vendor identifier
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }

        /// <summary>
        /// Gets or sets the Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        public string Gtin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the product is gift card
        /// </summary>
        public bool IsGiftCard { get; set; }

        #region Price
        /// <summary>
        /// Gets or sets a value indicating whether to show "Call for Pricing" or "Call for quote" instead of price
        /// </summary>
        public bool CallForPrice { get; set; }
        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the old price
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// Gets or sets auction start price
        /// </summary>
        public decimal StartPrice { get; set; }
        /// <summary>
        /// Gets or sets auction end price
        /// </summary>
        public decimal EndPrice { get; set; }
        #endregion

        #region Weight Length Width Height
        /// <summary>
        /// Gets or sets the weight
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// Gets or sets the length
        /// </summary>
        public decimal Length { get; set; }
        /// <summary>
        /// Gets or sets the width
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public decimal Height { get; set; }
        #endregion

        #region Condition
        /// <summary>
        /// Gets or sets the order minimum quantity
        /// </summary>
        public int OrderMinimumQuantity { get; set; }
        /// <summary>
        /// Gets or sets the order maximum quantity
        /// </summary>
        public int OrderMaximumQuantity { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the product allows customer reviews
        /// </summary>
        public bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this product is marked as new
        /// </summary>
        public bool MarkAsNew { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }
        #endregion

        #region SEO Properties
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }
        #endregion
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IList<string> Stores { get; set; }
        public IList<ProductCategoryMappingView> ProductCategoryMapping { get; set; }
        public string BrandId { get; set; }
        public string SpeakerId { get; set; }
        public IList<ImageView> Images { get; set; }
        public IList<ProductAttributeMappingView> Attributes { get; set; }
        public IList<ProductVariantView> Variants { get; set; }
        public string Location { get; set; }

        public DateTime? StartDateUtc { get; set; }
        public SpeakerView? Speaker { get; set; }
        public List<string> Notes { get; set; }
    }
}
