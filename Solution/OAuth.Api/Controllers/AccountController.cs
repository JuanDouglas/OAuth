using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly OAuthContext db = new();

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

    }
}
