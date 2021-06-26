using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
