namespace TimeNet.Areas.Admin.Models.Datatable;

public class DataTableResponseModel
{
    public long draw { get; set; }
    public long recordsTotal { get; set; }
    public long recordsFiltered { get; set; }
    public object data { get; set; }
    public string message { get; set; }
}