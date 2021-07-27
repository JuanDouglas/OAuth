using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers.Base;
using OAuth.Api.Models;
using OAuth.Api.Models.Attributes;
using OAuth.Api.Models.Enums;
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
            await ValidModelAsync(accountModel);
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

            if (LoginController.ValidPassword(pas, account.Password))
                return Unauthorized(AttempType.PasswordIncorrect);

            #region UpdateModel 
            account.Email = string.IsNullOrEmpty(accountModel.Email) ? account.Email : accountModel.Email;
            account.PhoneNumber = string.IsNullOrEmpty(accountModel.PhoneNumber) ? account.PhoneNumber : accountModel.Email;
            account.UserName = string.IsNullOrEmpty(accountModel.UserName) ? account.UserName : accountModel.UserName;
            account.IsCompany = accountModel.IsCompany ?? account.IsCompany;

            accountModel.Email = account.Email;
            accountModel.PhoneNumber = account.PhoneNumber;
            accountModel.UserName = account.UserName;
            accountModel.IsCompany = account.IsCompany;

            if (!string.IsNullOrEmpty(accountModel.Password))
                account.Password = LoginController.HashPassword(accountModel.Password);
            #endregion

            #region ValidModel
            await ValidModelAsync(accountModel);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            db.Accounts.Update(account);
            await db.SaveChangesAsync();

            account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == id);

            return Ok(new Models.Result.Account(account));
        }

        private async Task ValidModelAsync(Models.Uploads.AccountUpload accountModel)
        {
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

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);
            return Ok(new Models.Result.Account(account));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Confirmation key</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VerifyAccount")]
        [RequireAuthentication]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Redirect)]
        public async Task<ActionResult> VerifyAccount(string key, string email)
        {
            AccountConfirmation confirmation = await db.AccountConfirmations.FirstOrDefaultAsync(fs => fs.Key == key && fs.AccountNavigation.Email == email);

            if (confirmation == null)
                return NotFound();

            if ((DateTime.UtcNow - confirmation.Date).TotalMinutes > 15)
                return Forbid();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == confirmation.Account && fs.Email == email);
            if (account == null)
                return NotFound();

            account.Valid = true;

            db.Accounts.Update(account);
            await db.SaveChangesAsync();

            string redirect = "https://" + Request.Host + $"/index.html";

            return Redirect(redirect);
        }

        /// <summary>
        /// Send e-mail confirnmation.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SendConfirmation")]
        [RequireAuthentication]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendConfirmation()
        {
            if (!Login.IsValid)
                return Unauthorized();

            Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Key == Login.AccountKey);

            AccountConfirmation confirmation = new()
            {
                Account = account.Id,
                Key = LoginController.GenerateToken(LoginController.LargerTokenSize),
                Date = DateTime.UtcNow
            };

            string confirmationLink = "https://" + Request.Host + $"/api/Account/VerifyAccount?key={confirmation.Key}&email={account.Email}";

            await db.AccountConfirmations.AddAsync(confirmation);
            await db.SaveChangesAsync();

            Smtp.SendEmail(account.Email,
                "Nexus Account Confirmation",
                Properties.Resources.confirm_account.Replace("{confirmation-link}", confirmationLink),
                true);

            return Ok();
        }
    }
}
