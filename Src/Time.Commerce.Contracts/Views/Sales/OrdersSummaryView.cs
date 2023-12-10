namespace Time.Commerce.Contracts.Views.Sales;

public class OrdersSummaryView
{
    public int LastDayOrders { get; set; }
    public decimal LastDaySales { get; set; }
    public int TodayOrders { get; set; }
    public decimal TodaySales { get; set; }
}
