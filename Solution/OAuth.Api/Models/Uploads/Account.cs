using OAuth.Api.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static OAuth.Api.Controllers.LoginController;

namespace OAuth.Api.Models.Uploads
{
    public class Account
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ConfirmPassword { get; set; }
        public string Email { get; set; }
        public bool IsCompany { get; set; }

        public Account()
        {
        }

        public Dal.Models.Account GetAccountDB()
        {
            return new Dal.Models.Account()
            {
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
                AcceptTermsDate = DateTime.UtcNow,
                UserName = UserName,
                CreateDate = DateTime.UtcNow,
                Key = GenerateToken(LargerTokenSize),
                Email = Email,
                IsCompany = IsCompany
            };
        }
    }
}
