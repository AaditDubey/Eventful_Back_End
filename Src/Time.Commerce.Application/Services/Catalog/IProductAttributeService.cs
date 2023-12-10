using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Catalog;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface IProductAttributeService
    {
        Task<ProductAttributeView> CreateAsync(CreateProductAttributeModel model, CancellationToken cancellationToken = default);
        Task<ProductAttributeView> UpdateAsync(UpdateProductAttributeModel model, CancellationToken cancellationToken = default);
        Task<bool> BulkUpdateAsync(IEnumerable<ProductAttribute> attributes, IEnumerable<string>? updateFields, CancellationToken cancellationToken = default);
        Task<ProductAttributeView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageableView<ProductAttributeView>> FindAsync(ProductAttributeQueryModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductAttribute>> GetAll(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
