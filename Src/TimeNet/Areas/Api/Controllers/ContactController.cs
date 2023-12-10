using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Enums.Identity;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Identity;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class ContactController : BaseApiController
{
    private readonly IContactMessageService _contactMessageService;
    public ContactController(IContactMessageService contactMessageService)
        => _contactMessageService = contactMessageService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateContactMessageModel model, CancellationToken cancellationToken = default)
    {
        model.Type = ContactMessageType.Message;
        return Ok(await _contactMessageService.CreateContactMessageAsync(model, cancellationToken));
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] CreateContactMessageModel model, CancellationToken cancellationToken = default)
    {
        model.Type = ContactMessageType.Subscribe;
        return Ok(await _contactMessageService.CreateContactMessageAsync(model, cancellationToken));
    }
}