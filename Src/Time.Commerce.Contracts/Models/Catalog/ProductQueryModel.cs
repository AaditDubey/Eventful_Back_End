using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Catalog
{
    public class ProductQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public string SpeakerId { get; set; }
        public string CategoryId { get; set; }
        public bool? Published { get; set; }
        public List<string> Ids { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class AttributeQueryModel
    {
        public string Id { get; set; }
        public string Values { get; set; }
    }
}
