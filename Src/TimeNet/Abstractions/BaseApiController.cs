using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Core.Constants;

namespace TimeNet.Abstractions;

[ApiController]
[Route(CoreSystemConst.API_ROUTE_FORMAT)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status200OK)]
public abstract class BaseApiController : ControllerBase
{
}
