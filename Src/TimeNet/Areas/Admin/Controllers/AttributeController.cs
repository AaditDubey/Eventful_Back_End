using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Views.Catalog;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class AttributeController : BaseCookieAuthController
{
    #region Fields
    private readonly IProductAttributeService _productAttributeService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public AttributeController(IProductAttributeService productAttributeService, IWorkContext workContext)
    {
        _productAttributeService = productAttributeService;
        _workContext = workContext;
    }
    #endregion

    #region Mvc Actions
    public IActionResult Index(string searchText)
    {
        ViewBag.SearchText = searchText;
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    public IActionResult Update(string id)
    {
        ViewBag.Id = id;
        return View();
    }
    #endregion

    #region Apis
    [HttpPost]
    public async Task<IActionResult> Get(DataTableRequestModel<ProductAttributeQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        ProductAttributeQueryModel productAttributeQueryModel = new ProductAttributeQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            Published = model.FilterModel.Published,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var productAttributes = await _productAttributeService.FindAsync(productAttributeQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<ProductAttributeView>(productAttributes);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] ProductAttributeQueryModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var productAttributes = await _productAttributeService.FindAsync(model, cancellationToken);
        return Ok(productAttributes);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _productAttributeService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateProductAttributeModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        return Ok(await _productAttributeService.CreateAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductAttributeModel model, CancellationToken cancellationToken)
      => Ok(await _productAttributeService.UpdateAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _productAttributeService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _productAttributeService.DeleteManyAsync(ids, cancellationToken));
    #endregion
}