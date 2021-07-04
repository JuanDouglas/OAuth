using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Android.Models.Upload
{
    [Serializable]
    public class LoginApp
    {
        public string AuthorizationKey { get; set; }
        public string AuthenticationToken { get; set; }
        public int AccountID { get; set; }

        public LoginApp()
        {

        }
        public LoginApp(ApiAuthentication apiAuthentication)
        {
            AuthorizationKey = apiAuthentication.AuthorizationToken;
            AuthenticationToken = apiAuthentication.AuthenticationToken;
            AccountID = apiAuthentication.AccountID;
        }
    }
}
