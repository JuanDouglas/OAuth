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
using Account = OAuth.Dal.Models.Account;
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
                return Unauthorized(AttempType.AuthorizeApp);


            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == Login.AuthenticationToken &&
                fs.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);
            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            Application app = await db.Applications.FirstOrDefaultAsync(fs => fs.Key == app_key);

            if (app == null)
                return NotFound();

            Models.Result.Authorization result;
            Authorization authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.ApplicationNavigation.Key == app_key &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

            /*
             * Validates that the application has already been authorized if it has already 
             * been authorised and the level of authorisation required is 
             * higher than the autual will increase the level of authorisation, 
             * otherwise ira case returns HTTP 409 Conflict.
             */
            if (authorization != null)
            {
                if (((AuthorizationLevel)authorization.Level) < level)
                {
                    if (authorization.Active)
                    {
                        authorization.Level = (int)level;
                        db.Authorizations.Update(authorization);
                        await db.SaveChangesAsync();

                        result = new(authorization) { AccountID = account.Id };
                        return Ok(result);
                    }
                }
                else
                {
                    return Conflict("This app has already been authorized for this account");
                }
            }

            authorization = new()
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
            authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.Key == authorization.Key &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

            result = new(authorization) { AccountID = account.Id };

            if (redirect)
                return Redirect(result.Redirect);

            return Ok(result);
        }

        [HttpGet]
        [RequireAuthentication]
        [Route("GetAuthorization")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Models.Result.Authorization), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Authorization>> GetAuthorization(string app_key)
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            Authorization authorization = await db.Authorizations.FirstOrDefaultAsync(fs => fs.ApplicationNavigation.Key == app_key &&
                fs.AuthenticationNavigation.LoginFirstStepNavigation.AccountNavigation.Key == Login.AccountKey);

            if (authorization == null)
                return NotFound();

            Models.Result.Authorization result = new() { AccountID = account.Id };
            return Ok(result);
        }


    }
}