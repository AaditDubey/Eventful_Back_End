using Time.Commerce.Contracts.Views.Common;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Extensions;

public static class DataTableExtensions
{
    public static PagingInput ParseToPagingInput<T>(this DataTableRequestModel<T> dataTable)
    {
        PagingInput pagingInput = new PagingInput()
        {
            Descending = dataTable.order?.ElementAt(0)?.dir.Equals("desc") ?? false,
            Page = (dataTable.start / dataTable.length) + 1,
            PageSize = dataTable.length
        };
        Column orderColumn = (dataTable.order != null && dataTable.order.FirstOrDefault() != null) ? dataTable.columns[dataTable.order[0].column] : new Column();
        //if(!string.IsNullOrEmpty(orderColumn.data))
        //    orderColumn.data = char.ToUpper(orderColumn.data[0]) + orderColumn.data.Substring(1);
        pagingInput.OrderBy = string.IsNullOrEmpty(orderColumn.name) ? orderColumn.data : orderColumn.name;

        return pagingInput;
    }

    public static void GetFromPagingResult<T>(this DataTableResponseModel dataTableResponse, PageableView<T> pagedResult)
    {
        dataTableResponse.data = pagedResult.Items;
        dataTableResponse.recordsTotal = pagedResult.TotalItems;
        dataTableResponse.recordsFiltered = pagedResult.TotalItems;
    }
}
