using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Catalog;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface ICategoryService
    {
        Task<CategoryView> CreateAsync(CreateCategoryModel model, CancellationToken cancellationToken = default);
        Task<bool> BulkInsertAsync(IEnumerable<Category> categories, CancellationToken cancellationToken = default);
        Task<CategoryView> UpdateAsync(UpdateCategoryModel model, CancellationToken cancellationToken = default);
        Task<bool> BulkUpdateAsync(IEnumerable<Category> categories, IEnumerable<string> updateFields, CancellationToken cancellationToken = default);
        Task<CategoryView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CategoryView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<PageableView<CategoryView>> FindAsync(CategoryQueryModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryView>> GetAllWithParentAsync(CategoryQueryModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryRecursiveView>> GetAllWithRecursive(CategoryQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<CategoryView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
        Task<CategoryView> DeleteImage(string id, CancellationToken cancellationToken = default);
    }
}
