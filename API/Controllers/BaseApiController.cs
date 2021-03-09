using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Reactivities.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BaseApiController : ControllerBase
  {
    private IMediator _mediator;

    // ??=  nullの場合  手動で追加　using Microsoft.Extensions.DependencyInjection;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices
        .GetService<IMediator>();
  }
}
