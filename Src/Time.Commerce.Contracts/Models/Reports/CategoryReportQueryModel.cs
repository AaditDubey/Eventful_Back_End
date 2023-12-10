using Time.Commerce.Contracts.Models.Catalog;

namespace Time.Commerce.Contracts.Models.Reports;

public class CategoryReportQueryModel : CategoryQueryModel
{
    public string Language { get; set; } = "en";
}
