namespace Time.Commerce.Contracts.Views.Sales
{
    public partial class ShoppingCartView
    {
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public string CustomerId { get; set; }
        public string StoreId { get; set; }

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
        public IList<ShoppingCartItemView> Items { get; set; }
        public double SubTotal { get; set; }
        public double DiscountAmount { get; set; }
        public double ShippingAmount { get; set; }
        public double Total { get; set; }
    }
}