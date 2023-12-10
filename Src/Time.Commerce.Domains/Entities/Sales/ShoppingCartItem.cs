using AspNetCoreExtensions.Exceptions;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Sales
{
    /// <summary>
    /// Represents a shopping cart item
    /// </summary>
    public partial class ShoppingCartItem : SubBaseEntity
    {
        private int quantity = 0;
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the custom attributes (see "ProductAttribute" entity for more info)
        /// </summary>
        public IList<CustomAttribute> Attributes { get; set; } = new List<CustomAttribute>();

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity {
            get => quantity;
            set
            {
                if (value > 0)
                    quantity = value;
                else
                    throw new BadRequestException(nameof(Quantity), "Quantity must be greater than 0");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the shopping cart item is free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }

        /// <summary>
        /// Gets a value indicating whether the shopping cart item is gift card
        /// </summary>
        public bool IsGiftCard { get; set; }
    }
}
