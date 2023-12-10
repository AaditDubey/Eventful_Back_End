using Time.Commerce.Contracts.Models.Catalog;

namespace Time.Commerce.Contracts.Models.Reports;

public class ProductReportQueryModel : ProductQueryModel
{
    public string Language { get; set; } = "en";
}