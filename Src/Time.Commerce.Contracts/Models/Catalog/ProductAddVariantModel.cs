using FluentValidation;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class ProductAddVariantModel
    {
        public List<GenericAttributeModel> Attributes { get; set; }
        public decimal Price { get; set; }
        public string ImageId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string QrCode { get; set; }
        public string WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductAddVariantModelValidator : AbstractValidator<ProductAddVariantModel>
    {
        public ProductAddVariantModelValidator()
        {
            RuleFor(x => x.Attributes).NotNull().NotEmpty();
        }
    }
}
