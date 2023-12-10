using DocumentFormat.OpenXml.Office2010.Excel;
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
public class StoreController : BaseCookieAuthController
{
    #region Fields
    private readonly IStoreService _storeService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public StoreController(IStoreService storeService, IWorkContext workContext)
    {
        _storeService = storeService;
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
    public async Task<IActionResult> Get(DataTableRequestModel<StoreQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        StoreQueryModel storeQueryModel = new StoreQueryModel
        {
            //Published = model.FilterModel.Published,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var stores = await _storeService.FindAsync(storeQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<StoreView>(stores);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] StoreQueryModel model, CancellationToken cancellationToken)
    {
        //model.StoreId = _workContext.GetCurrentStoreId();
        var stores = await _storeService.FindAsync(model, cancellationToken);
        return Ok(stores);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _storeService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateStoreModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        return Ok(await _storeService.CreateAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStoreModel model, CancellationToken cancellationToken)
      => Ok(await _storeService.UpdateAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _storeService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _storeService.DeleteManyAsync(ids, cancellationToken));


    [HttpPost]
    public async Task<IActionResult> AddLogo(string id, [FromBody] ImageModel model, CancellationToken cancellationToken)
    {
        return Ok(await _storeService.AddImage(id, model, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLogo(string id, CancellationToken cancellationToken)
    {
        return Ok(await _storeService.DeleteImage(id, cancellationToken));
    }
    #endregion
}