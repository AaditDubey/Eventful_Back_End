using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Contracts.Views.Catalog;

public class ProductSimpleView
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SeName { get; set; }
    public string Sku { get; set; }
    public string ShortDescription { get; set; }
    public string VendorId { get; set; }
    public bool IsGiftCard { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public ImageView? Image { get; set; }
}
