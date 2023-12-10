namespace Time.Commerce.Domains.Entities.Catalog
{
    /// <summary>
    /// Represents a product type
    /// </summary>
    public enum ProductType
    {
        Unspecified = 0,
        /// <summary>
        /// Item has weight and needs shipping or customer pickup
        /// </summary>
        Phycical = 1,
        /// <summary>
        /// Item is delivered digitally or provided as a service
        /// </summary>
        Digital = 2,
        /// <summary>
        /// Item is a group of several physical or digital products
        /// </summary>
        Bundled = 3,
        /// <summary>
        /// Generates a unique code for store credit when purchased
        /// </summary>
        GiftCard = 4,
        /*

        /// <summary>
        /// Simple
        /// </summary>
        SimpleProduct = 5,
        /// <summary>
        /// Grouped (product with variants)
        /// </summary>
        GroupedProduct = 10,

        /// <summary>
        /// Reservation product
        /// </summary>
        Reservation = 15,

        /// <summary>
		/// Bundled product
		/// </summary>
		BundledProduct = 20,

        /// <summary>
        /// Auction
        /// </summary>
        Auction = 25

        */
    }
}
