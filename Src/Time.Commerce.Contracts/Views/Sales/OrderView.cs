using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Sales
{
    public partial class OrderView : BaseAuditView
    {
        #region Properties

        #region Infor
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the order code 
        /// </summary>
    
        public string Note { get; set; }
        #endregion

        #region Store
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        #endregion

        #region Customer infor
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the CustomerEmail
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the url referrer exists
        /// </summary>
        public string UrlReferrer { get; set; }
        /// <summary>
        /// Gets or sets the customer tax display type identifier
        /// </summary>
        public int CustomerTaxDisplayTypeId { get; set; }
        /// <summary>
        /// Gets or sets the customer currency code (at the moment of order placing)
        /// </summary>
        public string CustomerCurrencyCode { get; set; }
        /// <summary>
        /// Gets or sets the customer language identifier
        /// </summary>
        public string CustomerLanguageId { get; set; }
        /// <summary>
        /// Gets or sets the customer IP address
        /// </summary>
        public string CustomerIp { get; set; }
        #endregion

        #region Affiliate
        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public string AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        #endregion

        #region credit card info
        /// <summary>
        /// Gets or sets a value indicating whether storing of credit card number is allowed
        /// </summary>
        public bool AllowStoringCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card type
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the card name
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Gets or sets the card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the masked credit card number
        /// </summary>
        public string MaskedCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card CVV2
        /// </summary>
        public string CardCvv2 { get; set; }

        /// <summary>
        /// Gets or sets the card expiration month
        /// </summary>
        public string CardExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the card expiration year
        /// </summary>
        public string CardExpirationYear { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the owner identifier
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a customer chose "pick up in store" shipping option
        /// </summary>
        public bool PickUpInStore { get; set; }

        /// <summary>
        /// Gets or sets an order status identifier
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the shipping status identifier
        /// </summary>
        public string ShippingStatus { get; set; }

        /// <summary>
        /// Gets or sets the payment status identifier
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the payment method system name
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the primary currency code (at the moment of order placing)
        /// </summary>
        public string PrimaryCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the currency rate
        /// </summary>
        public decimal CurrencyRate { get; set; }

        /// <summary>
        /// Gets or sets the currency rate
        /// </summary>
        public decimal Rate { get; set; }


        /// <summary>
        /// Gets or sets the VAT number 
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the VAT number status id
        /// </summary>
        public int VatNumberStatusId { get; set; }

        /// <summary>
        /// Gets or sets the order subtotal (incl tax)
        /// </summary>
        public decimal OrderSubtotalInclTax { get; set; }

        /// <summary>
        /// Gets or sets the order subtotal (excl tax)
        /// </summary>
        public decimal OrderSubtotalExclTax { get; set; }

        /// <summary>
        /// Gets or sets the order subtotal discount (incl tax)
        /// </summary>
        public decimal OrderSubTotalDiscountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the order subtotal discount (excl tax)
        /// </summary>
        public decimal OrderSubTotalDiscountExclTax { get; set; }

        /// <summary>
        /// Gets or sets the order shipping (incl tax)
        /// </summary>
        public decimal OrderShippingInclTax { get; set; }

        /// <summary>
        /// Gets or sets the order shipping (excl tax)
        /// </summary>
        public decimal OrderShippingExclTax { get; set; }

        /// <summary>
        /// Gets or sets the payment method additional fee (incl tax)
        /// </summary>
        public decimal PaymentMethodAdditionalFeeInclTax { get; set; }

        /// <summary>
        /// Gets or sets the payment method additional fee (excl tax)
        /// </summary>
        public decimal PaymentMethodAdditionalFeeExclTax { get; set; }

        /// <summary>
        /// Gets or sets the order tax
        /// </summary>
        public decimal OrderTax { get; set; }

        /// <summary>
        /// Gets or sets the order discount (applied to order total)
        /// </summary>
        public decimal OrderDiscount { get; set; }

        /// <summary>
        /// Gets or sets the order total
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the refunded amount
        /// </summary>
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether reward points were earned for this order
        /// </summary>
        public bool RewardPointsWereAdded { get; set; }

        /// Gets or sets the value indicating for calculated reward points 
        public int CalcRewardPoints { get; set; }

        /// <summary>
        /// Gets or sets the checkout attribute description
        /// </summary>
        public string CheckoutAttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction identifier
        /// </summary>
        public string AuthorizationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction code
        /// </summary>
        public string AuthorizationTransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction result
        /// </summary>
        public string AuthorizationTransactionResult { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction identifier
        /// </summary>
        public string CaptureTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction result
        /// </summary>
        public string CaptureTransactionResult { get; set; }

        /// <summary>
        /// Gets or sets the subscription transaction identifier
        /// </summary>
        public string SubscriptionTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the paid date and time
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the shipping method
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Gets or sets the shipping rate computation method identifier
        /// </summary>
        public string ShippingRateComputationMethodSystemName { get; set; }

        /// <summary>
        /// Gets or sets the serialized CustomValues (values from ProcessPaymentRequest)
        /// </summary>
        public string CustomValuesXml { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been imported
        /// </summary>
        public bool Imported { get; set; }

        /// <summary>
        /// Gets or sets the Shipping Option description (customer friendly string)
        /// </summary>
        public string ShippingOptionAttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the Shipping Option in XML format (developer friendly string)
        /// </summary>
        public string ShippingOptionAttributeXml { get; set; }
        #endregion

        #region Navigation properties

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        public virtual AddressView BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the shipping address
        /// </summary>
        public virtual AddressView ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets order items
        /// </summary>
        public IList<OrderItemView> OrderItems { get; set; }
        public IList<OrderTaxView> OrderTaxes { get; set; }
        public IList<string> OrderTags { get; set; }

        #endregion
    }
}

