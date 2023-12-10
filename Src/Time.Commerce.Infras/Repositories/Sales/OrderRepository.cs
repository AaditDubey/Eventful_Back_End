using Time.Commerce.Domains.Entities.Sales;
using Time.Commerce.Domains.Repositories.Sales;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Sales
{
    public class OrderRepository : MongoRepository<Order>, IOrderRepository
    {
        public OrderRepository(IMongoContext context) : base(context)
        {
        }
    }
}
