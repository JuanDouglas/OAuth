using Microsoft.EntityFrameworkCore;
using OAuth.Dal;
using OAuth.Dal.Models;
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
            Authentication authentication = await db.Authentications.FirstOrDefaultAsync(fs => fs.Token == AuthenticationToken &&
            fs.LoginFirstStepNavigation.Token == FirstStepKey &&
            fs.LoginFirstStepNavigation.AccountNavigation.Key == AccountKey);

            if (authentication == null)
                return false;

            return true;

        }
    }

}
