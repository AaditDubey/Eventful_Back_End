using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Identity;

public class ContactMessageRepository : MongoRepository<ContactMessage>, IContactMessageRepository
{
    public ContactMessageRepository(IMongoContext context) : base(context)
    {
    }
}
