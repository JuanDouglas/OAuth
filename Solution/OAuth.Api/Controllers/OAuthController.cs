using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class OAuthController : ControllerBase
    {
        public const string ReplaceAuthorizationToken = "{authorization-token}";
        public const string ReplaceAccountID = "{account-id}";
        private readonly OAuthContext db = new();

        /// <summary>
        /// Get Application Authorization 
        /// </summary>
        /// <param name="app_key">Application Key</param>
        /// <param name="level">Authorization Level</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Authorize")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Models.Result.Authorization), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AuthorizeAsync(string app_key, AuthorizationLevel level, bool redirect)
        {
            LoginInformations login = LoginController.ValidInformations(Request);
            if (!login.IsValid)
            {
                return Unauthorized();
            }

            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == login.AuthenticationToken && fs.LoginFirstStepNavigation.Account == login.AccountID);
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
    }
}