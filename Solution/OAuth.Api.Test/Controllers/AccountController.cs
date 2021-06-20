using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OAuth.Api.Controllers;
using OAuth.Api.Models.Uploads;
using OAuth.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Api.Test.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private readonly OAuthContext db = new();

        [TestMethod]
        public async Task TestCreate()
        {
            AccountController accountController = new();

            Account uploadAccount = new()
            {
                UserName = "JuanDouglas",
                Password = "Am4@0309",
                AcceptTerms = true,
                ConfirmPassword = "Am4@0309",
                Email = "juandouglas2004@gmail.com",
                IsCompany = true
            };

            ActionResult<Models.Result.Account> actionResult = await accountController.CreateAsync(uploadAccount);
        await    actionResult.Result.ExecuteResultAsync(new ());
            Models.Result.Account resultAccount = actionResult.Value;

            Assert.IsNotNull(resultAccount);
            Assert.IsTrue(CompareResult(resultAccount, uploadAccount), "Account result not equal account upload");

            Dal.Models.Account dbAccount = await db.Accounts.FirstOrDefaultAsync(fs=>fs.Email==uploadAccount.Email);

            Assert.IsNotNull(dbAccount);
            Assert.IsTrue(CompareResult(new(dbAccount), uploadAccount),"Account db not equal account upload.");
            //Models.Result.Account resultAccount = JsonConvert.DeserializeObject<Models.Result.Account>();
        }


        private bool CompareResult(Models.Result.Account account, Account accountCompare)
        {
            if (account.IsCompany == accountCompare.IsCompany)
                return false;

            if (account.UserName == accountCompare.UserName)
                return false;

            if (account.Email == accountCompare.Email)
                return false;

            if ((account.CreateDate - DateTime.UtcNow).TotalMinutes > 2)
                return false;
            return true;
        }

    }
}
