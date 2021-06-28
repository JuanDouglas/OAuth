using OAuth.Dal.Models;
using System;

namespace OAuth.Api.Models.Result
{
    public class ValidLogin
    {
        public ValidLogin(ApplicationAuthentication authentication)
        {
            LoginDate = authentication.Date;
            IsValid = authentication.Active;
        }

        public DateTime ValidationDate { get; set; }
        public DateTime LoginDate { get; set; }
        public bool IsValid { get; set; }
    }
}
