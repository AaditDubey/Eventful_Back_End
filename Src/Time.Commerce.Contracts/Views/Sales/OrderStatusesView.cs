namespace Time.Commerce.Contracts.Views.Sales
{
    public class OrderStatusesView
    {
        public IEnumerable<string> OrderStatus { get; set; }
        public IEnumerable<string> PaymentStatus { get; set; }
        public IEnumerable<string> ShippingStatus { get; set; }

    }
}
