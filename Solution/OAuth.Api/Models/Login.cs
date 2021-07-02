using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Threading.Tasks;

namespace OAuth.Api.Models
{
    public class Login
    {
        public bool IsValid
        {
            get
            {
                Task<bool> validLogin = ValidLogin();
                return validLogin.Result;
            }
        }
        public string AuthenticationToken { get; set; }
        public string AccountKey { get; set; }
        public string FirstStepKey { get; set; }
        private readonly OAuthContext db = new();
        public Login(string accountKey, string authenticationToken, string firstStepKey)
        {
            AuthenticationToken = authenticationToken;
            AccountKey = accountKey;
            FirstStepKey = firstStepKey;
        }

        private async Task<bool> ValidLogin()
        {
            try
            {
                Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == AuthenticationToken);
                if (authentication == null)
                    return false;

                LoginFirstStep firstStep = await db.LoginFirstSteps.FirstOrDefaultAsync(fs => fs.Id == authentication.LoginFirstStep);
                if (firstStep == null)
                    return false;

                if (!LoginController.ValidPassword(FirstStepKey, firstStep.Token))
                    return false;

                Account account = await db.Accounts.FirstOrDefaultAsync(fs => fs.Id == firstStep.Account);
                if (account == null)
                    return false;

                if (account.Key != AccountKey)
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;

        }
    }

}
