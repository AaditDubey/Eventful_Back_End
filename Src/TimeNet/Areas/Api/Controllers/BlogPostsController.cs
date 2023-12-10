using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Identity;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class BlogPostsController : BaseApiController
{
    private readonly IBlogPostService _blogPostService;
    public BlogPostsController(IBlogPostService blogPostService)
        => _blogPostService = blogPostService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] BlogPostQueryModel model, CancellationToken cancellationToken = default)
    {
        return Ok(await _blogPostService.FindAsync(model, cancellationToken));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlugAsync([FromRoute] string slug, CancellationToken cancellationToken = default)
    {
        return Ok(await _blogPostService.GetBySlugAsync(slug, cancellationToken));
    }
}