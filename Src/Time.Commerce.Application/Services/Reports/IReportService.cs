using Microsoft.AspNetCore.Http;
using Time.Commerce.Contracts.Models.Reports;

namespace Time.Commerce.Application.Services.Reports;

public interface IReportService
{
    Task<byte[]> ExportCategoryReportAsync(CategoryReportQueryModel parameters,
        CancellationToken cancellationToken = default);

    Task<byte[]> DownloadCategoryReportTemplateAsync(string language,
        CancellationToken cancellationToken = default);

    Task<bool> ImportCategoryReportAsync(IFormFile file, string language,
        CancellationToken cancellationToken = default);

    Task<byte[]> ExportBrandReportAsync(BrandReportQueryModel parameters,
        CancellationToken cancellationToken = default);

    Task<byte[]> ExportProductReportAsync(ProductReportQueryModel parameters,
        CancellationToken cancellationToken = default);
}