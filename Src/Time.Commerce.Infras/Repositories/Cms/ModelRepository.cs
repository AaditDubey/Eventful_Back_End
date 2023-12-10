using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Cms
{
    public class ModelRepository : MongoRepository<Model>, IModelRepository
    {
        public ModelRepository(IMongoContext context) : base(context)
        {
        }
    }
}
