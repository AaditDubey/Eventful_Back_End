using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Repositories;

namespace Time.Commerce.Infras.Repositories.Identity
{
    public class RefreshTokenRepository : MongoRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IMongoContext context) : base(context)
        {
        }
    }
}
