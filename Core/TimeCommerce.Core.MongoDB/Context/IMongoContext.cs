using MongoDB.Driver;

namespace TimeCommerce.Core.MongoDB.Context
{
    public interface IMongoContext
    {
        MongoClient GetClient();
        IMongoDatabase GetDatabase();
        void DropDatabase();
        IMongoCollection<T> GetCollection<T>(string name);
        string GetConnection();
    }
}
