using Time.Commerce.Domains.Entities.Cms;

namespace Time.Commerce.Application.Services.Cms
{
    public interface IModelService
    {
        Task<Model> CreateAsync(Model model, CancellationToken cancellationToken = default);
        Task<Model> UpdateAsync(Model model, CancellationToken cancellationToken = default);
        Task<Model> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Model> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Model>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
