using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class VendorController : BaseCookieAuthController
{
    #region Fields
    private readonly IVendorService _vendorService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public VendorController(IVendorService vendorService, IWorkContext workContext)
    {
        _vendorService = vendorService;
        _workContext = workContext;
    }
    #endregion

    #region Mvc Actions
    public IActionResult Index()
    {
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
    public async Task<IActionResult> Get(DataTableRequestModel<VendorQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        VendorQueryModel vendorQueryModel = new VendorQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            Active = model.FilterModel.Active,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var vendors = await _vendorService.FindAsync(vendorQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<VendorView>(vendors);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] VendorQueryModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var vendors = await _vendorService.FindAsync(model, cancellationToken);
        return Ok(vendors);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _vendorService.GetVendorByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateVendorModel model, CancellationToken cancellationToken)
    {
        return Ok(await _vendorService.CreateVendorAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateVendorModel model, CancellationToken cancellationToken)
      => Ok(await _vendorService.UpdateVendorAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _vendorService.DeleteVendorAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _vendorService.DeleteManyAsync(ids, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> AddImage(string id, [FromBody] ImageModel model, CancellationToken cancellationToken)
    {
        return Ok(await _vendorService.AddImage(id, model, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(string id, CancellationToken cancellationToken)
    {
        return Ok(await _vendorService.DeleteImage(id, cancellationToken));
    }
    #endregion
}