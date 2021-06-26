using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models;
using OAuth.Api.Models.Enums;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Authorization = OAuth.Dal.Models.Authorization;

namespace OAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ApiController
    {
        public const string ReplaceAuthorizationToken = "{authorization-token}";
        public const string ReplaceAccountID = "{account-id}";

        /// <summary>
        /// Get Application Authorization 
        /// </summary>
        /// <param name="app_key">Application Key</param>
        /// <param name="level">Authorization Level</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Authorize")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Models.Result.Authorization), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AuthorizeAsync(string app_key, AuthorizationLevel level, bool redirect)
        {
            if (!Login.IsValid)
            {
                return Unauthorized();
            }

            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == Login.AuthenticationToken &&
                fs.LoginFirstStepNavigation.Account == Login.AccountID);
            Application app = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key);
            if (app == null)
            {
                return NotFound();
            }

            Authorization authorization = new()
            {
                Key = LoginController.GenerateToken(LoginController.LargerTokenSize),
                Date = DateTime.UtcNow,
                Level = (int)level,
                Active = true,
                Application = app.Id,
                Authentication = authentication.Id
            };

            await db.Authorizations.AddAsync(authorization);
            await db.SaveChangesAsync();
            authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.Key == authorization.Key && fs.Authentication == authentication.Id);

            Models.Result.Authorization result = new(authorization);

            if (redirect)
                return Redirect(result.Redirect);

            return Ok(new Models.Result.Authorization(authorization));
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> LoginApp(string authorization_token, string app_key)
        {
            bool containsUserAgent = HttpContext.Request.Headers.TryGetValue("User-Agent", out StringValues userAgent);
            if (!containsUserAgent)
                return BadRequest("User-Agent is mandatory");
            

            if (!Login.IsValid)
                return Unauthorized();

            Authorization authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.Key == authorization_token &&
                fs.ApplicationNavigation.Key == app_key &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.Account == Login.AccountID);
            if (authorization == null)
                return Unauthorized();

            Application application = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key);
            if (application == null)
                return NotFound();

            ApplicationAuthentication appAuth = new()
            {
                Application = application.Id,
                Date = DateTime.UtcNow,
                Ipadress = HttpContext.Connection.RemoteIpAddress.ToString(),
                Token = LoginController.GenerateToken(LoginController.LargerTokenSize),
                 UserAgent = userAgent.ToString(),
Authentication =                  
            };


            throw new NotImplementedException();
        }
    }
}