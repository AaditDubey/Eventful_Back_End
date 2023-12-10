using AutoMapper;
using Microsoft.AspNetCore.Http;
using Time.Commerce.Application.Constants.Reports;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Reports;
using Time.Commerce.Contracts.Enums.Reports;
using Time.Commerce.Contracts.Models.Reports;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Domains.Entities.Catalog;

namespace Time.Commerce.Infras.Services.Reports;

public class ReportService : IReportService
{
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ReportService(ICategoryService categoryService, IBrandService brandService, IProductService productService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _brandService = brandService;
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<byte[]> ExportCategoryReportAsync(CategoryReportQueryModel parameters,
        CancellationToken cancellationToken = default)
    {
        var data = await _categoryService.FindAsync(parameters, cancellationToken);
        LanguageConstants.HeaderLanguages.TryGetValue(ReportType.Category, out var headerLanguage);
        return data.Items.ExportToExcel(parameters.Language, headerLanguage, "Categories");
    }

    public async Task<byte[]> DownloadCategoryReportTemplateAsync(string language,
        CancellationToken cancellationToken = default)
    {
        var categoryView = new List<CategoryView>();
        LanguageConstants.HeaderLanguages.TryGetValue(ReportType.Category, out var headerLanguage);
        return categoryView.ExportToExcel(language, headerLanguage, "Categories");
    }

    public async Task<bool> ImportCategoryReportAsync(IFormFile file, string language,
        CancellationToken cancellationToken = default)
    {
        var isFoundHeader = LanguageConstants.HeaderLanguages.TryGetValue(ReportType.Category, out var headerLanguage);
        if (!isFoundHeader)
            return false;
        var listData = await file.GetImportExcelDataAsync<CategoryView>(language, headerLanguage, "Categories");

        var categoryViews = listData.ToList();
        var listInsert = categoryViews.Where(x => string.IsNullOrEmpty(x.Id)).ToList();
        if (listInsert.Count > 0)
        {
            var listInsertEntities = _mapper.Map<IEnumerable<Category>>(listInsert);
            await _categoryService.BulkInsertAsync(listInsertEntities, cancellationToken);
        }

        var listUpdate = categoryViews.Where(x => !string.IsNullOrEmpty(x.Id)).ToList();
        if (listUpdate.Count > 0)
        {
            var listKey = headerLanguage[language].Select(x => x.Key).ToList();
            var listUpdateEntities = _mapper.Map<IEnumerable<Category>>(listUpdate);
            await _categoryService.BulkUpdateAsync(listUpdateEntities, listKey, cancellationToken);
        }

        return true;
    }

    public async Task<byte[]> ExportBrandReportAsync(BrandReportQueryModel parameters,
        CancellationToken cancellationToken = default)
    {
        var data = await _brandService.FindAsync(parameters, cancellationToken);
        LanguageConstants.HeaderLanguages.TryGetValue(ReportType.Brand, out var headerLanguage);
        return data.Items.ExportToExcel(parameters.Language, headerLanguage, "Brands");
    }

    public async Task<byte[]> ExportProductReportAsync(ProductReportQueryModel parameters,
        CancellationToken cancellationToken = default)
    {
        var data = await _productService.FindAsync(parameters, cancellationToken);
        LanguageConstants.HeaderLanguages.TryGetValue(ReportType.Product, out var headerLanguage);
        return data.Items.ExportToExcel(parameters.Language, headerLanguage, "Products");
    }
}