using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Sales;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class ShoppingCartController : BaseApiController
{
    private readonly IShoppingCartService _shoppingCartService;
    public ShoppingCartController(IShoppingCartService shoppingCartService)
        => _shoppingCartService = shoppingCartService;

    [HttpPost("createEmptyCart")]
    public async Task<IActionResult> CreateEmptyCart(string? storeId, string? customerId, CancellationToken cancellationToken = default)
       => Ok(await _shoppingCartService.CreateEmptyCartAsync(storeId, customerId, cancellationToken));

    [HttpPost("{id}:addProductsToCart")]
    public async Task<IActionResult> AddProductsToCart(string id, [FromBody] List<ShoppingCartItemModel> model, CancellationToken cancellationToken = default)
        => Ok(await _shoppingCartService.AddProductsToCartAsync(id, model, cancellationToken));

    [HttpPut("{id}:updateCartItems")]
    public async Task<IActionResult> UpdateCartItems(string id, [FromBody] List<ShoppingCartItemModel> model, CancellationToken cancellationToken = default)
       => Ok(await _shoppingCartService.UpdateCartItemsAsync(id, model, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        => Ok(await _shoppingCartService.GetShoppingCartViewWithProductInforAsync(id, cancellationToken));

    [Route("{id}/item/{itemId}")]
    [HttpDelete]
    public async Task<IActionResult> RemoveCartItems(string id, string itemId, CancellationToken cancellationToken = default)
        => Ok(await _shoppingCartService.RemoveCartItemsAsync(id, itemId, cancellationToken));
}
