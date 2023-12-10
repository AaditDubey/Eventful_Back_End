using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Application.Services.Cms
{
    public interface IContentService
    {
        Task<ContentView> CreateAsync(CreateContentModel model, CancellationToken cancellationToken = default);
        Task<ContentView> UpdateAsync(UpdateContentModel model, CancellationToken cancellationToken = default);
        Task<ContentView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ContentView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<PageableView<ContentView>> FindAsync(ContentQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<ContentView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<ContentView> DeleteImage(string id, CancellationToken cancellationToken = default);
    }
}
