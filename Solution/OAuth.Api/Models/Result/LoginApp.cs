using OAuth.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Result
{
    public class LoginApp
    {
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public Application Application { get; set; }
        public string AuthorizationKey { get; set; }
        public string AuthenticationToken { get; set; }

        private readonly OAuthContext db = new();
        public LoginApp()
        {

        }

        public LoginApp(Dal.Models.ApplicationAuthentication appAuth)
        {
            Dal.Models.Application application = db.Applications.FirstOrDefault(fs => fs.Id == appAuth.Application);
            Dal.Models.Authentication authentication = db.Authentications.FirstOrDefault(fs => fs.Id == appAuth.Authentication);
            Dal.Models.Authorization authorization = db.Authorizations.FirstOrDefault(fs => fs.Id == appAuth.Authorization);

            IPAdress = appAuth.Ipadress;
            Date = appAuth.Date;
            Application = new(application);
            UserAgent = appAuth.UserAgent;
            AuthorizationKey = authorization.Key;
            AuthenticationToken = authentication.Token;
        }
    }
}
