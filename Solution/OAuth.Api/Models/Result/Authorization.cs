using OAuth.Api.Controllers;
using OAuth.Api.Models.Enums;
using OAuth.Dal;
using System;
using System.Linq;

namespace OAuth.Api.Models.Result
{
    public class Authorization
    {
        public Application Application { get; set; }
        public string Key { get; set; }
        public bool Active { get; set; }
        public AuthorizationLevel Level { get; set; }
        public DateTime Date { get; set; }
        public int AccountID { get; set; }
        public string Redirect
        {
            get
            {
                return _redirect.Replace(OAuthController.ReplaceAuthorizationToken, Key)
                    .Replace(OAuthController.ReplaceAccountID, AccountID.ToString());
            }
        }
        private string _redirect;

        private readonly OAuthContext db = new();
        public Authorization()
        {

        }
        public Authorization(Dal.Models.Authorization authorization)
        {
            Dal.Models.Application application = db.Applications.FirstOrDefault(fs => fs.Id == authorization.Id);

            Key = authorization.Key;
            Level = (AuthorizationLevel)authorization.Level;
            Active = authorization.Active;
            Date = authorization.Date;
            Application = new(application);
            _redirect = application.AuthorizeRedirect;
        }
    }
}
