using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Cms;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class ContentController : BaseCookieAuthController
{
    #region Fields
    private readonly IContentService _contentService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public ContentController(IContentService contentService, IWorkContext workContext)
    {
        _contentService = contentService;
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
    public async Task<IActionResult> Get(DataTableRequestModel<ContentQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        ContentQueryModel contentQueryModel = new ContentQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            Published = model.FilterModel.Published,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var contents = await _contentService.FindAsync(contentQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<ContentView>(contents);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] ContentQueryModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var contents = await _contentService.FindAsync(model, cancellationToken);
        return Ok(contents);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _contentService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateContentModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        return Ok(await _contentService.CreateAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateContentModel model, CancellationToken cancellationToken)
      => Ok(await _contentService.UpdateAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _contentService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _contentService.DeleteManyAsync(ids, cancellationToken));


    [HttpPost]
    public async Task<IActionResult> AddImage(string id, [FromBody] ImageModel model, CancellationToken cancellationToken)
    {
        return Ok(await _contentService.AddImage(id, model, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(string id, CancellationToken cancellationToken)
    {
        return Ok(await _contentService.DeleteImage(id, cancellationToken));
    }
    #endregion
}