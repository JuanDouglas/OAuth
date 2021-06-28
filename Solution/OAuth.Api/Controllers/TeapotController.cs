using Microsoft.AspNetCore.Mvc;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TeapotController : ControllerBase
    {
        [HttpGet]
        [Route("MakeCoffe")]
        public ActionResult MakeACoffe()
        {
            return StatusCode(418, "It is not possible to make coffee in a teapot.");
        }
    }
}
