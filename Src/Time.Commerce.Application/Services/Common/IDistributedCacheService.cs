namespace Time.Commerce.Application.Services.Common
{
    public interface IDistributedCacheService
    {
        Task<TData> GetAsync<TData>(string Key, CancellationToken cancellationToken = default) where TData : class;
        Task RemoveAsync(string Key, CancellationToken cancellationToken = default);
        Task SetAsync<TData>(string key, TData value, CancellationToken cancellationToken = default) where TData : class;
        Task SetAsync<TData>(string key, TData value, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class;
        Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, CancellationToken cancellationToken = default) where TData : class;
        Task<TData> GetOrCreateAsync<TData>(string key, Func<Task<TData>> factory, TimeSpan? expiredIn, CancellationToken cancellationToken = default) where TData : class;
    }
}
