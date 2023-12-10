using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class CategoryQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public bool? Published { get; set; }
        public bool? ShowOnHomePage { get; set; }
        public string[]? Ids { get; set; }
    }
}
