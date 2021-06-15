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
        [StringLength(100, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(500)] 
        public string Email { get; set; }
        public bool IsCompany { get; set; }
        public bool AcceptTerms { get; set; }

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
