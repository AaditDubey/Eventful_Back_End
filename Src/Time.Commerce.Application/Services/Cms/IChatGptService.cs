namespace Time.Commerce.Application.Services.Cms
{
    public interface IChatGptService
    {
        Task<string> ChatAsync(string message, CancellationToken cancellationToken = default);
    }
}
