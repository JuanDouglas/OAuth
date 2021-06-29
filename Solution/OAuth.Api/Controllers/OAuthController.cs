using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models;
using OAuth.Api.Models.Attributes;
using OAuth.Api.Models.Enums;
using OAuth.Api.Models.Result;
using OAuth.Dal.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Application = OAuth.Dal.Models.Application;
using Authentication = OAuth.Dal.Models.Authentication;
using Authorization = OAuth.Dal.Models.Authorization;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ApiController
    {
        public const string ReplaceAuthorizationToken = "{authorization-token}";
        public const string ReplaceAccountID = "{account-id}";
        public const string ReplaceAuthenticationToken = "{authentication-token}";
        public const string ReplaceLoginToken = "{login-token}"; 

        /// <summary>
        /// Get Application Authorization 
        /// </summary>
        /// <param name="app_key">Application Key</param>
        /// <param name="level">Authorization Level</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Authorize")]
        [RequireAuthentication]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Models.Result.Authorization), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Authorization>> AuthorizeAsync(string app_key, AuthorizationLevel level, bool redirect)
        {
            if (!Login.IsValid)
            {
                await RegisterFailAttempAsync(AttempType.AuthorizeApp);
                return Unauthorized(Login);
            }

            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == Login.AuthenticationToken &&
                fs.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

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
        [ProducesResponseType(typeof(LoginApp), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginApp>> LoginApp(string authorization_token, string app_key, bool redirect)
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
                Authorization = authorization.Id
            };

            await db.ApplicationAuthentications.AddAsync(appAuth);
            await db.SaveChangesAsync();

            appAuth = await db.ApplicationAuthentications.FirstOrDefaultAsync(fs => fs.Token == appAuth.Token &&
                fs.Authentication == appAuth.Authentication);

            LoginApp login = new(appAuth);

            if (redirect)
                return Redirect(login.Redirect);

            return Ok();
        }

        /// <summary>
        ///  Valid Application Authentication
        /// </summary>
        /// <param name="app_key">Application Key</param>
        /// <param name="private_key">Application Private Key</param>
        /// <param name="login">Login Informations</param>
        /// <returns></returns>
        [HttpGet]
        [RequireAuthentication]
        [Route("ValidAuthentication")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ValidLogin), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ValidLogin>> ValidLoginAsync(string app_key, string private_key, [FromBody] Models.Uploads.LoginUpload login)
        {
            if (!Login.IsValid)
                return Unauthorized(Login);

            Application application = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key &&
                fs.PrivateKey == private_key);

            ApplicationAuthentication authentication = await db.ApplicationAuthentications.FirstOrDefaultAsync(fs => fs.Token == login.AuthenticationToken &&
                fs.AuthorizationNavigation.Key == login.AuthorizationKey &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.Account == login.AccountID &&
                fs.ApplicationNavigation.Key == app_key &&
                fs.Active == true
            );

            if (application == null)
                return NotFound();

            if (authentication == null)
                return Unauthorized();

            return Ok(new ValidLogin(authentication)
            {
                ValidationDate = DateTime.UtcNow
            });
        }
    }
}