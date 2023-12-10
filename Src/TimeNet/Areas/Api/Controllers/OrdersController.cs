using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByNumber([FromRoute] string id, CancellationToken cancellationToken = default)
        => Ok(await _orderService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> GetByNumber([FromBody] CreateOrderModel model, CancellationToken cancellationToken = default)
        => Ok(await _orderService.CreateAsync(model, cancellationToken));
}
