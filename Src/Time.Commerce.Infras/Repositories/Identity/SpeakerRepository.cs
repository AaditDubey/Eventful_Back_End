using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Identity;

public class SpeakerRepository : MongoRepository<Speaker>, ISpeakerRepository
{
    public SpeakerRepository(IMongoContext context) : base(context)
    {
    }
}
