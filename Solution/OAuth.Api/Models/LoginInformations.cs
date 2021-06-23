using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models
{
    public class LoginInformations
    {
        public bool IsValid { get; set; }
        public int AccountID { get; set; }
        public string AuthenticationToken { get; set; }
        public string AccountKey { get; set; }

        public LoginInformations(int accountID, string accountKey, string authenticationToken)
        {
            AccountID = accountID;
            AuthenticationToken = authenticationToken;
            AccountKey = accountKey;
        }
    }
}
