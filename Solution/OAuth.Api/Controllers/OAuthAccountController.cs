using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers.Base;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Authorization = OAuth.Dal.Models.Authorization;

namespace OAuth.Api.Controllers
{
    [ApiController]
    [Route("api/OAuth/Account")]
    public class OAuthAccountController : ApiController
    {
        /// <summary>
        /// Get Account for authorization.
        /// </summary>
        /// <param name="auth_token">Authorization token.</param>
        /// <param name="app_key">Key for app.</param>
        /// <param name="account_id">Account ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(typeof(Models.Result.Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Account(string auth_token, string app_key, int account_id)
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account ownerAccount = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            Application application = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key && fs.Owner == ownerAccount.Id);

            if (application == null)
                return NotFound("Application NotFound");

            Authorization authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.Key == auth_token &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.Account == account_id &&
                fs.Application == application.Id);

            if (authorization == null)
                return NotFound("Authorization NotFound");

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == account_id);
            if (account == null)
                return NotFound();


            return Ok(new Models.Result.Account(account)
            {
                Key = "Private Information",
                UserName = "Private Information"
            });
        }
    }
}
