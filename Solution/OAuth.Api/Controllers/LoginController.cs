using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Api.Models.Enums;
using OAuth.Api.Models.Result;
using OAuth.Dal;
using OAuth.Dal.Models;
using Account = OAuth.Dal.Models.Account;

namespace OAuth.Api.Controllers
{
    /// <summary>
    /// Login Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        internal const int SmallTokenSize = 32;
        internal const int NormalTokenSize = 64;
        internal const int LargerTokenSize = 96;
        private readonly OAuthContext db = new();

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
        public async Task<ActionResult> FirstStepAsync(string user)
        {
            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.UserName == user || fs.Email == user);
            IPAddress ip = HttpContext.Connection.RemoteIpAddress;
            if (account == null)
            {
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

            return Ok(new FirstStep(loginFirstStep));
        }

        /// <summary>
        /// Second Step to Login.
        /// </summary>
        /// <param name="pwd">Password</param>
        /// <param name="key">First Step Key.</param>
        /// <returns>Authorization Tokens</returns>
        [HttpGet]
        [Route("SecondStep")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FirstStep))]
        public async Task<ActionResult> SecondStepAsync(string pwd, string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create new Account 
        /// </summary>
        /// <returns>New Account</returns>
        [HttpPut]
        [Route("Create")]
        public async Task<ActionResult> CreateAsync([FromBody] Models.Uploads.Account accountModel)
        {
            #region ValidModel
            if ((await db.Accounts.FirstOrDefaultAsync(fs => fs.UserName == accountModel.UserName)) != null)
            {
                ModelState.AddModelError("UserName", "The username is already being used.");
            }

            if ((await db.Accounts.FirstOrDefaultAsync(fs => fs.Email == accountModel.Email)) != null)
            {
                ModelState.AddModelError("Email", "The email is already being used.");
            }

            if (accountModel.ConfirmPassword != accountModel.Password)
            {
                ModelState.AddModelError("ConfirmPassword", "Password not equal at confirm password.");
            }

            if (!accountModel.AcceptTerms)
            {
                ModelState.AddModelError("AcceptTerms", "We must be accepted to continue registration.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            Account account = accountModel.ToAccountDB();
            db.Accounts.Add(account);

            await db.SaveChangesAsync();

            account = await db.Accounts.FirstOrDefaultAsync(fs => fs.UserName == accountModel.UserName);

            return Ok(new Models.Result.Account(account));
        }

        /// <summary>
        /// Generate Tokens with specific length
        /// </summary>
        /// <param name="size">Token Size</param>
        /// <returns>New token with size value.</returns>
        internal static string GenerateToken(int size)
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