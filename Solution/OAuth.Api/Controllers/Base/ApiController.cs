using Microsoft.AspNetCore.Mvc;
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
    public class ApiController : ControllerBase
    {
        public Login Login { get { return GetInformations(); } }
        protected internal OAuthContext db = new();

        public UnauthorizedResult Unauthorized(Login login)
        {
            if (!login.IsValid)
            {
                Task task = RegisterFailAttempAsync(AttempType.RequestLoginInvalid);
                task.Wait();
            }
            return base.Unauthorized();
        }

        /// <summary>
        /// Get login informations 
        /// </summary>
        /// <returns></returns>
        public Login GetInformations()
        {
            string authorizationToken = string.Empty;
            string accountKey = string.Empty;
            string firstStepKey = string.Empty;
            int accountId = 0;

            try
            {
                IEnumerator<KeyValuePair<string, StringValues>> headers = Request.Headers.GetEnumerator();
                do
                {
                    KeyValuePair<string, StringValues> header = headers.Current;
                } while (headers.MoveNext());
            }
            catch (NullReferenceException)
            {
                throw;
            }
            return new(accountId, accountKey, authorizationToken, firstStepKey);
        }

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
