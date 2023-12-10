using MongoDB.Bson;

namespace TimeCommerce.Core.MongoDB.Entities
{
    public abstract class BaseMongoEntity
    {
        protected BaseMongoEntity()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _id = ObjectId.GenerateNewId().ToString();
                else
                {
                    _ = new ObjectId();
                    _id = ObjectId.TryParse(value, out _) ? value : _id;
                }
            }
        }

        private string _id;

    }
}
