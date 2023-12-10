namespace TimeCommerce.Core.MongoDB.Models
{
    public partial class DataQuery
    {
        public string RawQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<SortOption> Sort { get; set; }
    }
}
