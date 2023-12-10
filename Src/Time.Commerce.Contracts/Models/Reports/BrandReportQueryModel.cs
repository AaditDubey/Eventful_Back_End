using Time.Commerce.Contracts.Models.Catalog;

namespace Time.Commerce.Contracts.Models.Reports;

public class BrandReportQueryModel : BrandQueryModel
{
    public string Language { get; set; } = "en";
}