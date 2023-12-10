using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Catalog
{
    public class ProductAttributeRepository : MongoRepository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(IMongoContext context) : base(context)
        {
        }
    }
}
