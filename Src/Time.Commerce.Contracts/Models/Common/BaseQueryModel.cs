namespace Time.Commerce.Contracts.Models.Common
{
    public class BaseQueryModel
    {
        private int pageIndex = 1;
        private int pageSize = 10;
        public string SearchText { get; set; }
        public int PageIndex
        {
            get => pageIndex;
            set
            {
                if (value > 0)
                    pageIndex = value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value > 0)
                    pageSize = value;
            }
        }
        public string OrderBy { get; set; }
        public bool Ascending { get; set; } = true;
    }
}
