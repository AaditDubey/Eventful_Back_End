using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class ProductAttributeQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public bool? Published { get; set; }
        public List<string>? Ids { get; set; }
    }
}
