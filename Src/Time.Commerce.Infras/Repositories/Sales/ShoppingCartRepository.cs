using Time.Commerce.Domains.Entities.Sales;
using Time.Commerce.Domains.Repositories.Sales;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Sales
{
    public class ShoppingCartRepository : MongoRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(IMongoContext context) : base(context)
        {
        }
    }
}
