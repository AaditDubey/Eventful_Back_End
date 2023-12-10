using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;

namespace Time.Commerce.Application.Services.Sales
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartView> CreateAsync(CreateShoppingCartModel model, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> UpdateAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> CreateOrUpdateAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageableView<ShoppingCartView>> FindAsync(ShoppingCartQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<string> CreateEmptyCartAsync(string storeId, string customerId = null, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> AddProductsToCartAsync(string cartId, List<ShoppingCartItemModel> items, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> UpdateCartItemsAsync(string cartId, List<ShoppingCartItemModel> items, CancellationToken cancellationToken = default);
        Task<ShoppingCartView> RemoveCartItemsAsync(string cartId, string cartItem, CancellationToken cancellationToken = default);
        Task<ShoppingCartView?> GetShoppingCartViewWithProductInforAsync(string cartId, CancellationToken cancellationToken = default);
    }
}
