using Time.Commerce.Domains.Entities.Sales;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Domains.Repositories.Sales
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
    }
}
