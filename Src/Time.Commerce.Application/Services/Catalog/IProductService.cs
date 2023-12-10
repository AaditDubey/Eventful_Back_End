using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Application.Services.Catalog
{
    public interface IProductService
    {
        Task<ProductView> CreateAsync(CreateProductModel model, CancellationToken cancellationToken = default);
        Task<ProductView> UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken = default);
        Task<ProductDetailsView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ProductDetailsView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<PageableView<ProductView>> FindAsync(ProductQueryModel model, CancellationToken cancellationToken = default);
        Task<PageableView<Product>> FindProductsAsync(ProductQueryModel model, CancellationToken cancellationToken = default);
        ProductVariant? GetSpecificVariant(Product product, IList<CustomAttribute> attributesOptions);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<ProductView> AddImages(string id, List<ImageModel> model, CancellationToken cancellationToken = default);
        Task<ProductView> DeleteImage(string id, string imageId, CancellationToken cancellationToken = default);
        Task<bool> CopyProduct(CopyProductModel model, CancellationToken cancellationToken = default);
        Task<ProductView> AddAttribute(string id, ProductAddAttributeModel model, CancellationToken cancellationToken = default);
        Task<ProductView> UpdateAttribute(string id, ProductUpdateAttributeModel model, CancellationToken cancellationToken = default);
        Task<ProductView> DeleteAttribute(string id, string attributeId, CancellationToken cancellationToken = default);
        List<KeyValuePair<string, string>> GetAttributeControlType(CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductOverViewView>> FindProductsOverViewAsync(ProductQueryModel model, CancellationToken cancellationToken = default);
        Task<ProductOverViewView> GetProductOverViewBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<ProductOverViewView> GetProductOverViewByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ProductView> AddVariant(string id, ProductAddVariantModel model, CancellationToken cancellationToken = default);
        Task<ProductView> UpdateVariant(string id, ProductUpdateVariantModel model, CancellationToken cancellationToken = default);
        Task<ProductView> DeleteVariant(string id, string variantId, CancellationToken cancellationToken = default);
        Task<ProductView> ApplyVariants(string id, IList<ProductAddVariantModel> variants, CancellationToken cancellationToken = default);
    }
}
