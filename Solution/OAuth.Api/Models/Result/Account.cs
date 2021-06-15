using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Result
{
    public class Account
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsCompany { get; set; }
        public string Key { get; set; }



        public Account()
        {
        }
        public Account(Dal.Models.Account account)
        {
            ID = account.Id;
            UserName = account.UserName;
            Email = account.Email;
            IsCompany = account.IsCompany;
            Key = account.Key;

        }

    }
}
