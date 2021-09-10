using OAuth.Api.Controllers;
using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Linq;

namespace OAuth.Api.Models.Result
{
    public class LoginApp
    {
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public DateTime Date { get; set; }
        public Application Application { get; set; }
        public string AuthorizationToken { get; set; }
        public string LoginToken { get; set; }
        public int AccountID { get; set; }
        public string Redirect { get; set; }
        private readonly OAuthContext db = new();
        public LoginApp()
        {

        }

        public LoginApp(Dal.Models.ApplicationAuthentication appAuth)
        {
            Dal.Models.Application application = db.Applications.FirstOrDefault(fs => fs.Id == appAuth.Application);
            Dal.Models.Authorization authorization = db.Authorizations.FirstOrDefault(fs => fs.Id == appAuth.Authorization);

            IPAdress = appAuth.Ipadress;
            Date = appAuth.Date;
            Application = new(application);
            LoginToken = appAuth.Token;
            UserAgent = appAuth.UserAgent;
            AuthorizationToken = authorization.Key;
            Redirect = application.LoginRedirect
                .Replace(OAuthController.ReplaceAuthorizationToken, AuthorizationToken)
                .Replace(OAuthController.ReplaceAccountID, AccountID.ToString())
                .Replace(OAuthController.ReplaceLoginToken, LoginToken);

        }
    }
}
