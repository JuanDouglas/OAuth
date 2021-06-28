using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models
{
    public class Login
    {
        public string FirstStepKey { get; set; }
        public int AccountID { get; set; }
        public string AuthenticationToken { get; set; }
        public bool IsValid { get; set; }
        public string AccountKey { get; set; }

        public Login(int accountID, string accountKey, string fsKey, string authenticationToken)
        {
            AccountID = accountID;
            AuthenticationToken = authenticationToken;
            AccountKey = accountKey;
            FirstStepKey = fsKey;
        }

    }
}
