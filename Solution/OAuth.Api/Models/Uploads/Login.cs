using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Uploads
{
    public class Login
    {
        public string AuthorizationKey { get; set; }
        public string LoginToken { get; set; }
        public int AccountID { get; set; }
    }
}
