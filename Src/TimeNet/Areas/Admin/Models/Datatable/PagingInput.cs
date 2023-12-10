namespace TimeNet.Areas.Admin.Models.Datatable;

public class PagingInput
{
    public PagingInput() { }

    public PagingInput(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public string OrderBy { get; set; }

    public bool Descending { get; set; }

}