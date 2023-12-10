using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Cms
{
    public class MessageRepository : MongoRepository<Message>, IMessageRepository
    {
        public MessageRepository(IMongoContext context) : base(context)
        {
        }
    }
}
