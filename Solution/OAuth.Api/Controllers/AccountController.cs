using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models.Attributes;
using OAuth.Dal.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OAuth.Api.Controllers
{
    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// Create new Account 
        /// </summary>
        /// <returns>New Account</returns>
        [HttpPut]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Models.Result.Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Account>> CreateAsync([FromBody] Models.Uploads.AccountUpload accountModel)
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
        ///  Update Account 
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="accountModel">Account Model</param>
        /// <param name="pas">Account Password</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [RequireAuthentication]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Models.Result.Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Account>> UpdateAsync(int id, string pas, [FromBody] Models.Uploads.AccountUpload accountModel)
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == id && fs.Key == Login.AccountKey);
            if (account == null)
                return NotFound();

            #region ValidModel
            if ((await db.Accounts.FirstOrDefaultAsync(fs => fs.UserName == accountModel.UserName)) != null)
            {
                ModelState.AddModelError("UserName", "The username is already being used.");
            }

            if ((await db.Accounts.FirstOrDefaultAsync(fs => fs.Email == accountModel.Email)) != null)
            {
                ModelState.AddModelError("Email", "The email is already being used.");
            }

            if ((await db.Accounts.FirstOrDefaultAsync(fs => fs.PhoneNumber == accountModel.PhoneNumber)) != null)
            {
                ModelState.AddModelError("PhoneNumber", "The PhoneNumber is already being used.");
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

            throw new NotImplementedException();
        }

        /// <summary>
        ///  Get Account 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [RequireAuthentication]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Models.Result.Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Models.Result.Account>> GetAsync()
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs=>fs.Key == Login.AccountKey);
            return Ok(new Models.Result.Account(account) );
        }
    }
}
