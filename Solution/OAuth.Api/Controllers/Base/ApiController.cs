﻿using Microsoft.AspNetCore.Mvc;
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
        public Login Login { get { return GetInformations(); } }
        protected internal OAuthContext db = OAuthDb;
        private static OAuthContext OAuthDb;

        public ApiController()
        {
            OAuthDb ??= new();

            if (OAuthDb != null)
                OAuthDb.SaveChangesFailed -= this.SaveChangesFailed;
            OAuthDb.SaveChangesFailed += this.SaveChangesFailed;
        }
        public override UnauthorizedResult Unauthorized()
        {
            return Unauthorized(Login);
        }

        [NonAction]
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
        [NonAction]
        private Login GetInformations()
        {
            string authorizationToken = string.Empty;
            string accountKey = string.Empty;
            string firstStepKey = string.Empty;

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

            return new(accountKey, authorizationToken, firstStepKey);
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
