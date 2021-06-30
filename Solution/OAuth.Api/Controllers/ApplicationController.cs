using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models.Attributes;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers
{
    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ApiController
    {
        [HttpPut]
        [Route("Add")]
        [RequireAuthentication]
        public async Task<ActionResult<Models.Result.ApplicationOwner>> AddAplication([FromBody] Models.Uploads.ApplicationUpload applicationUpload)
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            Application exits = await db.Applications.FirstOrDefaultAsync(fs => fs.Name == applicationUpload.Name &&
                fs.Owner == account.Id);

            if (exits != null)
            {
                ModelState.AddModelError("Name", "The Application Name is alreding begin used.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Application application = applicationUpload.ToApplicationDB();
            application.Owner = account.Id;

            await db.Applications.AddAsync(application);
            await db.SaveChangesAsync();

            application = await db.Applications.FirstOrDefaultAsync(fs => fs.Name == applicationUpload.Name &&
                fs.Owner == account.Id);

            return Ok(new Models.Result.ApplicationOwner(application));
        }

    }
}
