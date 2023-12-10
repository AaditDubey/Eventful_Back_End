using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Sales;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class OrderController : BaseCookieAuthController
{
    #region Fields
    private readonly IOrderService _orderService;
    private readonly IWorkContext _workContext;
    #endregion

    #region Ctor
    public OrderController(IOrderService orderService, IWorkContext workContext)
    {
        _orderService = orderService;
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
    public async Task<IActionResult> Get(DataTableRequestModel<OrderQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        OrderQueryModel orderQueryModel = new OrderQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            SearchText = model.FilterModel.SearchText,
            OrderStatus = model.FilterModel.OrderStatus,
            PaymentStatus = model.FilterModel.PaymentStatus,
            ShippingStatus = model.FilterModel.ShippingStatus,
            FromDate = model.FilterModel.FromDate,
            ToDate = model.FilterModel.ToDate,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var orders = await _orderService.FindAsync(orderQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<OrderView>(orders);

        return Ok(dataTableResponseModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] OrderQueryModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var orders = await _orderService.FindAsync(model, cancellationToken);
        return Ok(orders);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _orderService.GetByIdAsync(id, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetOrdersSummary(CancellationToken cancellationToken)
        => Ok(await _orderService.GetOrdersSummaryAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateOrderModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        return Ok(await _orderService.CreateAsync(model, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOrderModel model, CancellationToken cancellationToken)
      => Ok(await _orderService.UpdateAsync(model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _orderService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _orderService.DeleteManyAsync(ids, cancellationToken));

    [HttpGet]
    public IActionResult GetOrderStatuses(CancellationToken cancellationToken)
    => Ok(_orderService.GetOrderStatuses(cancellationToken));
    #endregion
}