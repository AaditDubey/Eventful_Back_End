using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Cms
{
    public class BlogPostRepository : MongoRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(IMongoContext context) : base(context)
        {
        }
    }
}
