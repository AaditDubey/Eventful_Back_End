using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Application.Services.Cms
{
    public interface IBlogPostService
    {
        Task<BlogPostView> CreateAsync(CreateBlogPostModel model, CancellationToken cancellationToken = default);
        Task<BlogPostView> UpdateAsync(UpdateBlogPostModel model, CancellationToken cancellationToken = default);
        Task<BlogPostDetailsView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<BlogPostDetailsView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<PageableView<BlogPostView>> FindAsync(BlogPostQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<BlogPostView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<BlogPostView> DeleteImage(string id, CancellationToken cancellationToken = default);
    }
}
