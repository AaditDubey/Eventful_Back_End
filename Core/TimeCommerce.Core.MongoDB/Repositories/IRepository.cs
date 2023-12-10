using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using TimeCommerce.Core.MongoDB.Entities;
using TimeCommerce.Core.MongoDB.Models;

namespace TimeCommerce.Core.MongoDB.Repositories
{
    public partial interface IRepository<T> where T : BaseMongoEntity
    {
        IMongoCollection<T> Collection { get; }
        Task<T> InsertAsync(T obj);
        Task<IEnumerable<T>> InsertListAsync(IEnumerable<T> objs);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> UpdateAsync(T obj);
        Task<bool> BulkUpdateAsync(IEnumerable<T> entities, IEnumerable<string> fieldsToUpdate);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteManyAsync(List<string> ids);
        Task<T> FillterSingleAsync(FilterDefinition<T> query);
        Task<IEnumerable<T>> FillterAsync(FilterDefinition<T> query);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);
        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> QueryAsync(DataFilterPaging<T> dataFilter);
        Task<DataFilterPagingResult<T>> CountAndQueryAsync(DataFilterPaging<T> dataFilter);
        Task<T> FindOneAsync(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where);
        IMongoQueryable<T> Table { get; }
    }
}
