namespace Time.Commerce.Contracts.Models.Sales
{
    public class CreateShoppingCartModel
    {
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public string StoreId { get; set; }
        public string CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the warehouse identifier
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart type identifier
        /// </summary>
        public string ShoppingCartType { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IList<ShoppingCartItemModel> Items { get; set; }
        public IList<string> CouponCodes { get; set; }
    }
}
