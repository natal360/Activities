using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")] //[] の外に api/ 
  public class BaseApiController : ControllerBase
  {

  }
}