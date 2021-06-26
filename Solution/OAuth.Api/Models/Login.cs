using Microsoft.EntityFrameworkCore;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int AccountID { get; set; }
        public string AuthenticationToken { get; set; }
        public string AccountKey { get; set; }
        public string FirstStepKey { get; set; }
        private readonly OAuthContext db = new();
        public Login(int accountID, string accountKey, string authenticationToken, string firstStepKey)
        {
            AccountID = accountID;
            AuthenticationToken = authenticationToken;
            AccountKey = accountKey;
            FirstStepKey = firstStepKey;
        }

        private async Task<bool> ValidLogin()
        {
            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == AuthenticationToken &&
            fs.LoginFirstStepNavigation.Token == FirstStepKey &&
            fs.LoginFirstStepNavigation.Account == AccountID &&
            fs.LoginFirstStepNavigation.AccountNavigation.Key == AccountKey);

            if (authentication == null)
                return false;

            return true;

        }
    }


}
