using MongoDB.Driver;
using TimeCommerce.Core.MongoDB.Settings;

namespace TimeCommerce.Core.MongoDB.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly MongoDataSettings _dataSettings;

        public MongoContext(MongoDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        public MongoClient GetClient()
        {
            return new MongoClient($"{_dataSettings.ConnectionString}/{_dataSettings.DatabaseName}");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return GetDatabase().GetCollection<T>(name); ;
        }

        public string GetConnection()
        {
            return !string.IsNullOrWhiteSpace(_dataSettings.ConnectionString) ? $"{_dataSettings.ConnectionString}/{_dataSettings.DatabaseName}" : string.Empty;
        }

        public IMongoDatabase GetDatabase()
        {
            return new MongoClient(_dataSettings.ConnectionString).GetDatabase(_dataSettings.DatabaseName);
        }

        public void DropDatabase()
        {
            new MongoClient(_dataSettings.ConnectionString).DropDatabase(_dataSettings.DatabaseName);
        }
    }
}
