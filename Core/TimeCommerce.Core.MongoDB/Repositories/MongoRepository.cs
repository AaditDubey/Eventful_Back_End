using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Entities;
using TimeCommerce.Core.MongoDB.Models;

namespace TimeCommerce.Core.MongoDB.Repositories
{
    public partial class MongoRepository<T> : IRepository<T> where T : BaseMongoEntity
    {
        #region Fields
        protected IMongoDatabase Database;
        protected IMongoCollection<T> DbSet;
        private readonly IMongoContext Context;
        private const string ID_FIELD_NAME = "Id";
        #endregion

        #region Ctor
        protected MongoRepository(IMongoContext context)
        {
            Context = context;
            if (!string.IsNullOrWhiteSpace(Context.GetConnection()))
            {
                Database = Context.GetDatabase();
                DbSet = Database.GetCollection<T>(typeof(T).Name);
            }
        }
        #endregion

        #region Async Method
        public IMongoCollection<T> Collection
        {
            get
            {
                return DbSet;
            }
        }

        public virtual async Task<T> InsertAsync(T obj)
        {
            await DbSet.InsertOneAsync(obj);
            return obj;
        }

        public virtual async Task<IEnumerable<T>> InsertListAsync(IEnumerable<T> objs)
        {
            await DbSet.InsertManyAsync(objs);
            return objs;
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            var data = await DbSet.Find(FilterIdAsync(id)).SingleOrDefaultAsync();
            return data;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var all = await DbSet.FindAsync(Builders<T>.Filter.Empty);
            return all.ToList();
        }

        public async virtual Task<T> UpdateAsync(T obj)
        {
            await DbSet.ReplaceOneAsync(FilterIdAsync(obj.Id), obj);
            return obj;
        }
        public async Task<bool> BulkUpdateAsync(IEnumerable<T> entities, IEnumerable<string> fieldsToUpdate)
        {
            var builder = Builders<T>.Update;
            var operations = (from entity in entities
                              let updates =
                                  (from field in fieldsToUpdate
                                   where field != "CreatedOn"
                                   let value = typeof(T).GetProperty(field)?.GetValue(entity)
                                   select builder.Set(field, value)).ToList()
                              let filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id)
                              let updatedByValue = typeof(T).GetProperty("UpdatedBy")?.GetValue(entity)
                              select new UpdateOneModel<T>(filter,
                                  builder.Combine(updates).CurrentDate("UpdatedOn").Set("UpdatedBy", updatedByValue ?? "System")))
                .Cast<WriteModel<T>>().ToList();

            var result = await DbSet.BulkWriteAsync(operations);
            return result.IsAcknowledged;
        }
        public async virtual Task<bool> DeleteAsync(string id)
        {
            var result = await DbSet.DeleteOneAsync(FilterIdAsync(id));
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteManyAsync(List<string> ids)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            foreach (var id in ids)
                objectIds.Add(ObjectId.Parse(id));
            var filter = Builders<T>.Filter.In(ID_FIELD_NAME, objectIds);
            var result = await DbSet.DeleteManyAsync(filter);
            return result.IsAcknowledged;
        }

        //public void Dispose()
        //{
        //    GC.SuppressFinalize(this);
        //}
        private static FilterDefinition<T> FilterIdAsync(string key)
        {
            return Builders<T>.Filter.Eq(ID_FIELD_NAME, ObjectId.Parse(key));
        }

        public async Task<T> FillterSingleAsync(FilterDefinition<T> query)
        {
            return await DbSet.FindAsync(query).Result.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FillterAsync(FilterDefinition<T> query)
        {
            var all = await DbSet.FindAsync(query);
            return all.ToList();
        }


        public virtual async Task<bool> AnyAsync()
        {
            return await DbSet.AsQueryable().AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.Find(where).AnyAsync();
        }

        public virtual async Task<long> CountAsync()
        {
            return await DbSet.CountDocumentsAsync(new BsonDocument());
        }
        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.CountDocumentsAsync(where);
        }

        public async Task<IEnumerable<T>> QueryAsync(DataFilterPaging<T> dataFilter)
        {
            var query = DbSet.Find(dataFilter.Filter).Skip((dataFilter.PageNumber - 1) * dataFilter.PageSize)
                                                                       .Limit(dataFilter.PageSize);

            if (dataFilter.Sort != null)
            {
                var sort = new SortDefinitionBuilder<T>();
                SortDefinition<T> sortDef = null;
                foreach (var sortable in dataFilter.Sort)
                {
                    FieldDefinition<T> field = sortable.Field;

                    if (sortable.Ascending)
                    {
                        sortDef = (sortDef == null) ? sort.Ascending(field) : sortDef.Ascending(field);
                    }
                    else
                    {
                        sortDef = (sortDef == null) ? sort.Descending(field) : sortDef.Descending(field);
                    }
                }
                query = query.Sort(sortDef);
            }

            return await query.ToListAsync();
        }

        public async Task<DataFilterPagingResult<T>> CountAndQueryAsync(DataFilterPaging<T> dataFilter)
        {
            return new DataFilterPagingResult<T>
            {
                Total = await CountAsync(dataFilter.Filter),
                Data = await QueryAsync(dataFilter)
            };
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.Find(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.Find(where).ToListAsync();
        }

        public virtual IMongoQueryable<T> Table
        {
            get { return DbSet.AsQueryable(); }
        }
        #endregion
    }
}
