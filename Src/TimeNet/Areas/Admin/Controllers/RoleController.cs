using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;

[Area("Admin")]
public class RoleController : BaseCookieAuthController
{
    #region Fields
    private readonly IRoleService _roleService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public RoleController(IRoleService roleService, IWorkContext workContext)
    {
        _roleService = roleService;
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
    public async Task<IActionResult> Get(DataTableRequestModel<RoleQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        RoleQueryModel roleQueryModel = new RoleQueryModel
        {
            Active = model.FilterModel.Active,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var roles = await _roleService.FindAsync(roleQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<RoleView>(roles);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] RoleQueryModel model, CancellationToken cancellationToken)
    {
        var roles = await _roleService.FindAsync(model, cancellationToken);
        return Ok(roles);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _roleService.GetRoleByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateRoleModel model, CancellationToken cancellationToken)
    {
        return Ok(await _roleService.CreateRoleAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoleModel model, CancellationToken cancellationToken)
      => Ok(await _roleService.UpdateRoleAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _roleService.DeleteRoleAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _roleService.DeleteManyAsync(ids, cancellationToken));

    #endregion
}