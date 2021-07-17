using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Models;
using OAuth.Api.Models.Enums;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers.Base
{
    [Controller]
    public abstract class ApiController : ControllerBase
    {
        public Login Login { get { return GetInformations(Request); } }
        protected internal OAuthContext db = new();


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
            }
            catch (NullReferenceException)
            {
               

            }
            return new(accountKey, authenticationToken, fsKey);
        }

        [NonAction]
        public async Task RegisterFailAttempAsync(AttempType attempType)
        {
            string ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            Ip ip = await db.Ips.FirstOrDefaultAsync(fs => fs.Adress == ipAdress);
            if (ipAdress == null)
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
