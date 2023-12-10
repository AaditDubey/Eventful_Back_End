using System.Collections.ObjectModel;

namespace Time.Commerce.Contracts.Views.Common
{
    public interface IPageable<T>
    {
    }

    public class PageableView<T> : IPageable<T>
    {
        public PageableView()
        {

        }
        public PageableView(int pageIndex, int pageSize, long totalItems, IList<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = new ReadOnlyCollection<T>(items);
            if (PageIndex > 0)
            {
                TotalPage = (TotalItems / PageSize);
                if (TotalItems % PageSize > 0)
                {
                    ++TotalPage;
                }
            }
        }
        public IReadOnlyCollection<T> Items { get; set; }
        public int PageIndex { get; set; }
        public long PageSize { get; set; }
        public long TotalItems { get; set; }
        public long TotalPage { get; set; } = 0;
    }
}
