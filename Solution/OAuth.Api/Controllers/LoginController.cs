using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models.Enums;
using OAuth.Api.Models.Result;
using OAuth.Dal.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Account = OAuth.Dal.Models.Account;
using Authentication = OAuth.Dal.Models.Authentication;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// Login Controller
    /// </summary>
    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ApiController
    {
        public const int SmallTokenSize = 32;
        public const int NormalTokenSize = 64;
        public const int LargerTokenSize = 96;
        public const string AuthorizationHeader = "Authorization";
        public const string AuthorizationTokenHeader = "Authorization-token";
        public const string AccountID = "Account-id";
        public const string AccountKey = "Account-Key";

        /// <summary>
        /// First Step to Login.
        /// </summary>
        /// <param name="user">Login UserName or Email</param>
        /// <returns>First Step Model</returns>
        [HttpGet]
        [Route("FirstStep")]
        [ProducesResponseType(typeof(FirstStep), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<FirstStep>> FirstStepAsync(string user)
        {
            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.UserName == user || fs.Email == user);
            IPAddress ip = HttpContext.Connection.RemoteIpAddress;
            if (account == null)
            {
                await RegisterFailAttempAsync(AttempType.UserInvalid);
                return NotFound();
            }

            if ((await db.Ips.FirstOrDefaultAsync(fs => fs.Adress == ip.ToString()) == null))
            {
                await db.Ips.AddAsync(new Ip()
                {
                    Adress = ip.ToString(),
                    AlreadyBeenBanned = false,
                    Confiance = ((int)IPConfiance.NONE)
                });
                await db.SaveChangesAsync();
            }

            LoginFirstStep loginFirstStep = new()
            {
                Account = account.Id,
                Date = DateTime.UtcNow,
                Token = GenerateToken(NormalTokenSize),
                Valid = true,
                Ipadress = ip.ToString()
            };

            db.LoginFirstSteps.Add(loginFirstStep);
            await db.SaveChangesAsync();

            return Ok(new FirstStep(await db.LoginFirstSteps.FirstOrDefaultAsync(fs => fs.Token == loginFirstStep.Token && fs.Valid)));
        }

        /// <summary>
        /// Second Step to Login.
        /// </summary>
        /// <param name="pwd">Account Password</param>
        /// <param name="key">First Step Key.</param>
        /// <param name="fs_id">First Step ID.</param>
        /// <returns>Authorization Tokens</returns>
        [HttpGet]
        [Route("SecondStep")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Models.Result.Authentication), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Authentication>> SecondStepAsync(string pwd, string key, int fs_id)
        {
            LoginFirstStep firstStep = await db.LoginFirstSteps.FirstOrDefaultAsync(fs => fs.Token == key && fs.Valid && fs.Id == fs_id);
            bool containsUserAgent = HttpContext.Request.Headers.TryGetValue("User-Agent", out StringValues userAgent);
            IPAddress ip = HttpContext.Connection.RemoteIpAddress;
            firstStep ??= new LoginFirstStep();

            if (!containsUserAgent)
            {
                return BadRequest("User-Agent is mandatory");
            }

            if (!firstStep.Valid)
            {
                await RegisterFailAttempAsync(AttempType.FirsStepInvalid);
                return Unauthorized();
            }

            //If login first step minutes > 5 minutes update firstep state with invalid.
            if ((DateTime.UtcNow - firstStep.Date).TotalMinutes > 5)
            {
                //Update login first step.
                firstStep.Valid = false;
                db.LoginFirstSteps.Update(firstStep);
                await db.SaveChangesAsync();
                return Unauthorized();
            }

            if (firstStep.Ipadress != ip.ToString())
            {
                return Unauthorized();
            }

            //Verify account password
            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == firstStep.Account);
            if (!ValidPassword(pwd, account.Password))
            {
                await RegisterFailAttempAsync(AttempType.PasswordIncorrect);
                return Unauthorized();
            }

            //Create authentication object
            Authentication authentication = new()
            {
                Date = DateTime.UtcNow,
                Ipadress = ip.ToString(),
                IsValid = true,
                Token = GenerateToken(LargerTokenSize),
                UserAgent = userAgent.ToString(),
                LoginFirstStep = firstStep.Id
            };

            var result = new Models.Result.Authentication(authentication);
            authentication.Token = HashPassword(result.Token);
            result.AccountID = account.Id;

            await db.Authentications.AddAsync(authentication);
            await db.SaveChangesAsync();

            //Update login first step.
            firstStep.Valid = false;
            db.LoginFirstSteps.Update(firstStep);
            await db.SaveChangesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Transform string password in string hash 
        /// </summary>
        /// <param name="password">String password</param>
        /// <returns>New hash by password</returns>
        [NonAction]
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Valid password by password hash.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="hash">Password hash.</param>
        /// <returns>password is valid</returns>
        [NonAction]
        public static bool ValidPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        /// <summary>
        /// Generate Tokens with specific length
        /// </summary>
        /// <param name="size">Token Size</param>
        /// <returns>New token with size value.</returns>
        [NonAction]
        public static string GenerateToken(int size)
        {
            string result = string.Empty;
            for (int i = 0; i < size / 32; i++)
            {
                result += Guid.NewGuid().ToString();
            }

            result = result.Replace("-", string.Empty);
            return result.Remove(size, result.Length - size);
        }
    }
}