using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Identity
{
    public class StoreRepository : MongoRepository<Store>, IStoreRepository
    {
        public StoreRepository(IMongoContext context) : base(context)
        {
        }
    }
}
