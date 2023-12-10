namespace Time.Commerce.Contracts.Enums.Sales;

/// <summary>
/// Represents the shipping status enumeration
/// </summary>
public enum ShippingStatus
{
    /// <summary>
    /// Shipping not required
    /// </summary>
    ShippingNotRequired = 10,
    /// <summary>
    /// Not yet shipped
    /// </summary>
    WaitingForDeliver = 20,//WaitingForDeliver
    /// <summary>
    /// Partially shipped
    /// </summary>
    PartiallyShipped = 25,
    /// <summary>
    /// Shipped
    /// </summary>
    Shipped = 30,
    /// <summary>
    /// Delivered
    /// </summary>
    //Delivered = 40,
}