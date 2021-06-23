using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeapotController : ControllerBase
    {
        [HttpGet]
        [Route("MakeCoffe")] 
        public ActionResult MakeACoffe() {
            return StatusCode(418, "It is not possible to make coffee in a teapot.");
        }
    }
}
