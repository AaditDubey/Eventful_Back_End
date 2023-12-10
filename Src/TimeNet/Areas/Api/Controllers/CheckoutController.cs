using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class CheckoutController : BaseApiController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderService _orderService;

    public CheckoutController(IShoppingCartService shoppingCartService, IOrderService orderService)
    {
        _shoppingCartService = shoppingCartService;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CheckoutModel model, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(model.CartId))
            return Ok(await _orderService.CreateWithShoppingCartAsync(model.CartId, model, cancellationToken));

        return Ok(await _orderService.CreateAsync(model, cancellationToken));
    }
}
