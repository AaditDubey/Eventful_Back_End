using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Views.Identity;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Api.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;
    private readonly ISpeakerService _speakerService;
    private readonly IMapper _mapper;
    public ProductsController(IProductService productService, ISpeakerService speakerService, IMapper mapper)
    {
        _productService = productService;
        _speakerService = speakerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProductQueryModel model, CancellationToken cancellationToken = default)
    {
        var products = await _productService.FindAsync(model, cancellationToken);
        if(products is not null && products.Items.Any())
        {
            var speakerIds = products.Items.Select(p => p.SpeakerId).ToList();
            var speakers = await _speakerService.FindAsync(new Time.Commerce.Contracts.Models.Identity.SpeakerQueryModel
            {
                Ids = speakerIds,
                PageSize = speakerIds.Count
            });

            if(speakers is not null && speakers.Items.Any() ) { 
                foreach(var product in products.Items)
                {
                    var speaker = speakers.Items.FirstOrDefault(x => x.Id == product.SpeakerId);
                    if (speaker is not null)
                        product.Speaker = _mapper.Map<SpeakerView>(speaker);
                }
            }
        }
        return Ok(products);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get([FromRoute] string slug, CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetBySlugAsync(slug, cancellationToken);
        if (product is not null && !string.IsNullOrEmpty(product.SpeakerId))
        {
            var speaker = await _speakerService.GetByIdAsync(product.SpeakerId, cancellationToken);
            if (speaker is not null)
                product.Speaker = _mapper.Map<SpeakerView>(speaker);
        }

        return Ok(product);
    }
}