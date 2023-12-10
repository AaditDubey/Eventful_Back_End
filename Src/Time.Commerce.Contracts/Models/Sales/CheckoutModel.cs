namespace Time.Commerce.Contracts.Models.Sales;

public class CheckoutModel : CreateOrderModel
{
    public string CartId { get; set; }
}
