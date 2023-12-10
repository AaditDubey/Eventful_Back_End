using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class BrandController : BaseCookieAuthController
{
    #region Fields
    private readonly IBrandService _brandService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public BrandController(IBrandService brandService, IWorkContext workContext)
    {
        _brandService = brandService;
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
    public async Task<IActionResult> Get(DataTableRequestModel<BrandQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        BrandQueryModel brandQueryModel = new BrandQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            Published = model.FilterModel.Published,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var brands = await _brandService.FindAsync(brandQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<BrandView>(brands);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] BrandQueryModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var brands = await _brandService.FindAsync(model, cancellationToken);
        return Ok(brands);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _brandService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBrandModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        return Ok(await _brandService.CreateAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBrandModel model, CancellationToken cancellationToken)
      => Ok(await _brandService.UpdateAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _brandService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _brandService.DeleteManyAsync(ids, cancellationToken));


    [HttpPost]
    public async Task<IActionResult> AddImage(string id, [FromBody] ImageModel model, CancellationToken cancellationToken)
    {
        return Ok(await _brandService.AddImage(id, model, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(string id, CancellationToken cancellationToken)
    {
        return Ok(await _brandService.DeleteImage(id, cancellationToken));
    }
    #endregion
}