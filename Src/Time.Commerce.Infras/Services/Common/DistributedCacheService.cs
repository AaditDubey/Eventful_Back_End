using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;
using Time.Commerce.Application.Services.Common;

namespace Time.Commerce.Infras.Services.Common
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IMemoryCache _memoryCache;
        public DistributedCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<TData> GetAsync<TData>(string key, CancellationToken cancellationToken = default) where TData : class
        {
            var cache = _memoryCache.TryGetValue(key, out TData data);
            return data;
        }
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
        
            _memoryCache.Remove(key);
        }

        public async Task SetAsync<TData>(string key, TData value, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class
        {
            _memoryCache.Set(key, value);
        }

        public async Task SetAsync<TData>(string key, TData value, CancellationToken cancellationToken = default) where TData : class
            => _memoryCache.Set(key, value);

        public async Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, CancellationToken cancellationToken = default) where TData : class
        {
            TData rs = await this.GetAsync<TData>(key, cancellationToken).ConfigureAwait(false);
            if ((object)rs != null)
                return rs;
            rs = await factory().ConfigureAwait(false);
            await this.SetAsync<TData>(key, rs, cancellationToken).ConfigureAwait(false);
            return rs;
        }

        public async Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class
        {
            TData rs = await this.GetAsync<TData>(key, cancellationToken).ConfigureAwait(false);
            if ((object)rs != null)
                return rs;
            rs = await factory().ConfigureAwait(false);
            await this.SetAsync<TData>(key, rs, expiredIn, cancellationToken).ConfigureAwait(false);
            return rs;
        }

        private byte[] GetDataAsByteArray<TData>(TData value)
        {
            var serializedData = JsonSerializer.Serialize(value);
            return Encoding.UTF8.GetBytes(serializedData);
        }
    }
}



//using Microsoft.Extensions.Caching.Distributed;
//using System.Text;
//using System.Text.Json;
//using Time.Commerce.Application.Services.Common;

//namespace Time.Commerce.Infras.Services.Common
//{
//    public class DistributedCacheService : IDistributedCacheService
//    {
//        private readonly IDistributedCache _distributedCache;
//        public DistributedCacheService(IDistributedCache distributedCache)
//        {
//            _distributedCache = distributedCache;
//        }

//        public async Task<TData> GetAsync<TData>(string key, CancellationToken cancellationToken = default) where TData : class
//        {
//            var dataAsByteArray = await _distributedCache.GetAsync(key, cancellationToken);
//            if ((dataAsByteArray?.Count() ?? 0) > 0)
//            {
//                var serializedData = Encoding.UTF8.GetString(dataAsByteArray);
//                var obj = JsonSerializer.Deserialize<TData>(serializedData);
//                return obj;
//            }
//            return null;
//        }
//        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
//        {
//            await _distributedCache.RemoveAsync(key, cancellationToken);
//        }

//        public async Task SetAsync<TData>(string key, TData value, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class
//        {
//            var expiration = new DistributedCacheEntryOptions
//            {
//                AbsoluteExpirationRelativeToNow = expiredIn
//            };
//            await _distributedCache.SetAsync(key, GetDataAsByteArray(value), expiration, cancellationToken);
//        }

//        public async Task SetAsync<TData>(string key, TData value, CancellationToken cancellationToken = default) where TData : class
//            => await _distributedCache.SetAsync(key, GetDataAsByteArray(value));

//        public async Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, CancellationToken cancellationToken = default) where TData : class
//        {
//            TData rs = await this.GetAsync<TData>(key, cancellationToken).ConfigureAwait(false);
//            if ((object)rs != null)
//                return rs;
//            rs = await factory().ConfigureAwait(false);
//            await this.SetAsync<TData>(key, rs, cancellationToken).ConfigureAwait(false);
//            return rs;
//        }

//        public async Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class
//        {
//            TData rs = await this.GetAsync<TData>(key, cancellationToken).ConfigureAwait(false);
//            if ((object)rs != null)
//                return rs;
//            rs = await factory().ConfigureAwait(false);
//            await this.SetAsync<TData>(key, rs, expiredIn, cancellationToken).ConfigureAwait(false);
//            return rs;
//        }

//        private byte[] GetDataAsByteArray<TData>(TData value)
//        {
//            var serializedData = JsonSerializer.Serialize(value);
//            return Encoding.UTF8.GetBytes(serializedData);
//        }
//    }
//}
