using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
