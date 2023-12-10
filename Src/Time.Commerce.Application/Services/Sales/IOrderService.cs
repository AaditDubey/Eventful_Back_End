using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;

namespace Time.Commerce.Application.Services.Sales
{
    public interface IOrderService
    {
        Task<OrderView> CreateAsync(CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<OrderView> CreateWithShoppingCartAsync(string cartId, CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<OrderView> UpdateAsync(UpdateOrderModel model, CancellationToken cancellationToken = default);
        Task<OrderView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageableView<OrderView>> FindAsync(OrderQueryModel model, CancellationToken cancellationToken = default);
        Task<OrdersSummaryView> GetOrdersSummaryAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        OrderStatusesView GetOrderStatuses(CancellationToken cancellationToken = default);
    }
}
