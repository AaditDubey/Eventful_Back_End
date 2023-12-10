using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Sales
{
    public class ShoppingCartItemView
    {
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string ProductId { get; set; }

        public ProductSimpleView Product { get; set; }    

        /// <summary>
        /// Gets or sets the custom attributes (see "ProductAttribute" entity for more info)
        /// </summary>
        public IList<CustomAttributeView> Attributes { get; set; } = new List<CustomAttributeView>();

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets a value indicating whether the shopping cart item is free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }

        /// <summary>
        /// Gets a value indicating whether the shopping cart item is gift card
        /// </summary>
        public bool IsGiftCard { get; set; }
        public double Amount { get; set; }
    }
}
