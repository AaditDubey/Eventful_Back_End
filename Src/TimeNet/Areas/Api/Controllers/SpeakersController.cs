using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class SpeakersController : BaseApiController
{
    private readonly ISpeakerService _speakerService;
    public SpeakersController(ISpeakerService speakerService)
        => _speakerService = speakerService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SpeakerQueryModel model, CancellationToken cancellationToken = default)
    {
        return Ok(await _speakerService.FindAsync(model, cancellationToken));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug, CancellationToken cancellationToken = default)
    {
        var sp = await _speakerService.GetBySlugAsync(slug, cancellationToken);
        return Ok(sp);
    }
}