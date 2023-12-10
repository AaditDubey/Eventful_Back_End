using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Views.Catalog
{
    public class ProductVariantView
    {
        public string ID { get; set; }  
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
}
