using TimeCommerce.Core.MongoDB.Entities;

namespace TimeCommerce.Core.MongoDB.Models
{
    public partial class DataFilterPagingResult<T> where T : BaseMongoEntity
    {
        public IEnumerable<T> Data { get; set; }
        public long Total { get; set; }
    }
}
