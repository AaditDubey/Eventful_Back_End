using System.Linq.Expressions;
using TimeCommerce.Core.MongoDB.Entities;

namespace TimeCommerce.Core.MongoDB.Models
{
    public partial class DataFilterPaging<T> where T : BaseMongoEntity
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
        public List<SortOption> Sort { get; set; }
    }
}
