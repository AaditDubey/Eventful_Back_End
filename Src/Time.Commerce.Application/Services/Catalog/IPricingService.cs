using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface IPricingService
    {
        Task<ShoppingCartView> CalculateTotalAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default);
        ShoppingCartItemView GetProductWithUnitPrice(Product product, IList<CustomAttribute> attributesOptions);
        ShoppingCartView CalculateCartTotalAsync(ShoppingCartView cart, CancellationToken cancellationToken = default);
    }
}
