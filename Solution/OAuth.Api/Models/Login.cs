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

    }
}
