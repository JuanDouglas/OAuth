using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Models.Attributes;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiController = OAuth.Api.Controllers.Base.ApiController;
using Authorization = OAuth.Dal.Models.Authorization;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// External Application Authentication OAuth Controller
    /// </summary>
    [RequireHttps]
    [ApiController]
    [Route("api/OAuth/Authentication")]
    public class OAuthAuthenticationController : ApiController
    {
        /// <summary>
        /// Get Authentication in especific App
        /// </summary>
        /// <param name="authorization_token">Application Authorization Token</param>
        /// <param name="app_key">Application Key</param>
        /// <returns></returns>
        [HttpGet]
        [RequireAuthentication]
        [Route("AppAuthentication")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Models.Result.LoginApp), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.LoginApp>> LoginApp(string authorization_token, string app_key, bool redirect)
        {
            bool containsUserAgent = HttpContext.Request.Headers.TryGetValue("User-Agent", out StringValues userAgent);

            if (!containsUserAgent)
                return BadRequest("User-Agent is mandatory");

            if (!Login.IsValid)
                return Unauthorized(Login);

            Authorization authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.Key == authorization_token &&
                fs.ApplicationNavigation.Key == app_key &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == Login.AuthenticationToken &&
                fs.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

            if (authorization == null || authentication == null)
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
                Authentication = authentication.Id,
                Authorization = authorization.Id,
                Active = true
            };

            await db.ApplicationAuthentications.AddAsync(appAuth);
            await db.SaveChangesAsync();

            appAuth = await db.ApplicationAuthentications.FirstOrDefaultAsync(fs => fs.Token == appAuth.Token &&
                fs.Authentication == appAuth.Authentication);

            Models.Result.LoginApp login = new(appAuth);

            if (redirect)
                return Redirect(login.Redirect);

            return Ok(login);
        }

        /// <summary>
        ///  Valid Application Authentication
        /// </summary>
        /// <param name="app_key">Application Key</param>
        /// <param name="login">Login Informations</param>
        /// <returns></returns>
        [HttpGet]
        [RequireAuthentication]
        [Route("ValidAuthentication")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Models.Result.ValidLogin), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.ValidLogin>> ValidLoginAsync([FromQuery] string app_key, [FromBody] Models.Uploads.LoginUpload login)
        {
            if (!Login.IsValid)
                return Unauthorized(Login);

            Application application = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key);
            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            ApplicationAuthentication authentication = await db.ApplicationAuthentications.FirstOrDefaultAsync(fs => fs.Token == login.AuthenticationToken);

            if (application == null)
                return NotFound();

            if (authentication == null)
                return NotFound();

            if (application.Owner != account.Id)
            {
                Authentication userAuthentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Id == authentication.Authentication);
                if (userAuthentication == null)
                    return NotFound();

                LoginFirstStep firstStep = await db.LoginFirstSteps.FirstOrDefaultAsync(fs => fs.Id == userAuthentication.LoginFirstStep);
                if (firstStep == null)
                    return NotFound();

                if (firstStep.Account != account.Id)
                    return NotFound();
            }

            return Ok(new Models.Result.ValidLogin(authentication)
            {
                ValidationDate = DateTime.UtcNow
            });
        }
    }
}
