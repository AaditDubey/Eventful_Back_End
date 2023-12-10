using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Catalog
{
    public class ProductRepository : MongoRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }
    }
}
