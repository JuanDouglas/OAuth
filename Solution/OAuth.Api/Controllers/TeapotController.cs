using Microsoft.AspNetCore.Mvc;
using OAuth.Api.Controllers.Base;
using System.Net;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// Teapot Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TeapotController : ApiController
    {
        [HttpGet]
        [Route("MakeCoffe")]
        [ProducesResponseType(418)]
        public ActionResult MakeACoffe()
        {
            return StatusCode(418, "It is not possible to make coffee in a teapot.");
        }
    }
}
