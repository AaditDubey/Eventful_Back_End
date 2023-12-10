using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Catalog
{
    public partial class Product : BaseAuditEntity, ILocalizedEntity
    {
        #region Fields
        private ICollection<ProductCategoryMapping> _productCategoryMapping;
        private ICollection<ProductAttributeMapping> _productAttributeMapping;
        private ICollection<ProductSpecificationAttributeMapping> _productSpecificationAttributes;
        private ICollection<ProductVariant> _variants;
        private ICollection<Image> _images;
        private ICollection<string> _appliedDiscounts;
        #endregion

        #region Ctor
        public Product()
        {
            Stores = new List<string>();
            Locales = new List<LocalizedProperty>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the product type identifier
        /// </summary>
        public int ProductTypeId { get; set; }
        /// <summary>
        /// Gets or sets the parent product identifier. It's used to identify associated products (only with "grouped" products)
        /// </summary>
        public string ParentGroupedProductId { get; set; }
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
        /// Gets or sets a vendor identifier
        /// </summary>
        public string VendorId { get; set; }
        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// Gets or sets the product type
        /// </summary>
        public ProductType ProductType
        {
            get
            {
                return (ProductType)this.ProductTypeId;
            }
            set
            {
                this.ProductTypeId = (int)value;
            }
        }
        /// <summary>
        /// Gets or sets the gift card type
        /// </summary>
        public GiftCardType GiftCardType
        {
            get
            {
                return (GiftCardType)this.GiftCardTypeId;
            }
            set
            {
                this.GiftCardTypeId = (int)value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }
        public IList<string> Stores { get; set; }
 
        /// <summary>
        /// Gets or sets the Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        public string Gtin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the product is gift card
        /// </summary>
        public bool IsGiftCard { get; set; }
        /// <summary>
        /// Gets or sets the gift card type identifier
        /// </summary>
        public int GiftCardTypeId { get; set; }

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

        #region StockQuantity
        /// <summary>
        /// Gets or sets the product cost
        /// </summary>
        public decimal ProductCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how to manage inventory
        /// </summary>
        public int ManageInventoryMethodId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether multiple warehouses are used for this product
        /// </summary>
        public bool UseMultipleWarehouses { get; set; }
        /// <summary>
        /// Gets or sets a warehouse identifier
        /// </summary>
        public string WarehouseId { get; set; }
        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to display stock availability
        /// </summary>
        public bool DisplayStockAvailability { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to display stock quantity
        /// </summary>
        public bool DisplayStockQuantity { get; set; }
        /// <summary>
        /// Gets or sets the minimum stock quantity
        /// </summary>
        public int MinStockQuantity { get; set; }
        /// <summary>
        /// Gets or sets the low stock 
        /// </summary>
        public bool LowStock { get; set; }

        /// <summary>
        /// Gets or sets the low stock activity identifier
        /// </summary>
        public int LowStockActivityId { get; set; }
        #endregion

        #region Shipping
        /// <summary>
        /// Gets or sets a value indicating whether the entity is ship enabled
        /// </summary>
        public bool IsShipEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the product allows customer reviews
        /// </summary>
        public bool AllowCustomerReviews { get; set; }
        public int TotalReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this product is marked as new
        /// </summary>
        public bool MarkAsNew { get; set; }
        

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the viewed
        /// </summary>
        public Int64 Viewed { get; set; }

        
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

        #region Navigation Properties
        /// <summary>
        /// Gets or sets the collection of ProductCategory
        /// </summary>
        public virtual ICollection<ProductCategoryMapping> ProductCategoryMapping
        {
            get { return _productCategoryMapping ??= new List<ProductCategoryMapping>(); }
            protected set { _productCategoryMapping = value; }
        }

        /// <summary>
        /// Gets or sets the collection of ProductAttribute
        /// </summary>
        public virtual ICollection<ProductAttributeMapping> ProductAttributeMapping
        {
            get { return _productAttributeMapping ??= new List<ProductAttributeMapping>(); }
            protected set { _productAttributeMapping = value; }
        }


        /// <summary>
        /// Gets or sets the product specification attribute
        /// </summary>
        public virtual ICollection<ProductSpecificationAttributeMapping> ProductSpecificationAttributes
        {
            get { return _productSpecificationAttributes ??= new List<ProductSpecificationAttributeMapping>(); }
            protected set { _productSpecificationAttributes = value; }
        }

        public string BrandId { get; set; }
        public string SpeakerId { get; set; }
        public string Location { get; set; }
        public DateTime? StartDateUtc { get; set; }


        /// <summary>
        /// Gets or sets the collection of ProductPicture
        /// </summary>
        public virtual ICollection<Image> Images
        {
            get { return _images ??= new List<Image>(); }
            protected set { _images = value; }
        }
        public virtual ICollection<ProductVariant> Variants
        {
            get { return _variants ??= new List<ProductVariant>(); }
            protected set { _variants = value; }
        }
        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        public virtual ICollection<string> AppliedDiscounts
        {
            get { return _appliedDiscounts ??= new List<string>(); }
            protected set { _appliedDiscounts = value; }
        }

        public List<string> Notes { get; set; }
        #endregion

        #region Locales

        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }
        #endregion
    }
}