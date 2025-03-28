using DatingAppApi.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    [ServiceFilter<LogUserActivity>]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
