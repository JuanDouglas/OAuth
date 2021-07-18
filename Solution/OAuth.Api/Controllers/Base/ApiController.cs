using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Models;
using OAuth.Api.Models.Enums;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers.Base
{
    [Controller]
    public abstract class ApiController : ControllerBase
    {
        public Login Login { get { return GetInformations(Request); } }
        protected internal OAuthContext db = new();

        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            var errorList = ModelState.ToDictionary(
      kvp => kvp.Key,
      kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            return base.BadRequest(new { status = 400, errors = errorList, title = "One or more validation errors occurred." });
        }
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object result)
        {
            return base.BadRequest(new { status = 400, errors = result, title = "One or more validation errors occurred." });
        }
        public override UnauthorizedResult Unauthorized()
        {
            return Unauthorized(Login);
        }

        [NonAction]
        public UnauthorizedResult Unauthorized(AttempType attempType)
        {
            Task task = RegisterFailAttempAsync(attempType);
            task.Wait();
            return base.Unauthorized();
        }

        [NonAction]
        public UnauthorizedResult Unauthorized(Login login)
        {
            if (!login.IsValid)
            {
                return Unauthorized(AttempType.RequestLoginInvalid);
            }

            return base.Unauthorized();
        }

        /// <summary>
        /// Get login informations 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public static Login GetInformations(HttpRequest httpRequest)
        {
            string
                authenticationToken = string.Empty,
                accountKey = string.Empty,
                fsKey = string.Empty;

            IHeaderDictionary headers = httpRequest.Headers;
            IRequestCookieCollection cookies = httpRequest.Cookies;
            headers.TryGetValue(LoginController.AuthenticationTokenHeader, out StringValues authorizationTokenSV);
            headers.TryGetValue(LoginController.AccountKeyHeader, out StringValues accountKeySV);
            headers.TryGetValue(LoginController.FirstStepKeyHeader, out StringValues firstStepKeySV);

            try
            {
                authenticationToken = authorizationTokenSV.ToString();
                accountKey = accountKeySV.ToString();
                fsKey = firstStepKeySV.ToString();

                if (authenticationToken == null ||
                    accountKey == null ||
                    fsKey == null)
                {
                    throw new NullReferenceException();
                }

                if (authenticationToken == string.Empty ||
                   accountKey == string.Empty ||
                   fsKey == string.Empty)
                {
                    throw new NullReferenceException();
                }
            }
            catch (NullReferenceException)
            {
                string cookie = httpRequest.Cookies[LoginController.CookieAuthetication];
                string[] values = cookie.Split(';');

                foreach (string keyPair in values)
                {
                    string[] pair = keyPair.Split('=');
                    if (pair.Length < 2)
                        break;

                    switch (pair[0])
                    {
                        case LoginController.AuthenticationTokenHeader:
                            authenticationToken = pair[1];
                            break;
                        case LoginController.AccountKeyHeader:
                            accountKey = pair[1];
                            break;
                        case LoginController.FirstStepKeyHeader:
                            fsKey = pair[1];
                            break;
                        default:
                            break;
                    }

                }
            }

            return new(accountKey, authenticationToken, fsKey);
        }

        [NonAction]
        public async Task RegisterFailAttempAsync(AttempType attempType)
        {
            string ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            Ip ip = await db.Ips.FirstOrDefaultAsync(fs => fs.Adress == ipAdress);
            if (ip == null)
            {
                ip = new()
                {
                    Adress = ipAdress,
                    AlreadyBeenBanned = false,
                    Confiance = (int)IPConfiance.NONE
                };

                await db.Ips.AddAsync(ip);
                await db.SaveChangesAsync();

                ip = await db.Ips.FirstOrDefaultAsync(fs => fs.Adress == ipAdress);
            }

            FailAttemp failAttemp = new()
            {
                AttempType = (int)attempType,
                Date = DateTime.UtcNow,
                Ipadress = ip.Adress
            };

            await db.FailAttemps.AddAsync(failAttemp);
            await db.SaveChangesAsync();
        }


    }
}
