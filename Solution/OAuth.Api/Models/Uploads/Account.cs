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
        private string _username;
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string UserName { get { return _username; } set { _username = value.ToLowerInvariant(); } }
        [Required]
        [StringLength(25, MinimumLength = 8)]
        [Password]
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

        public Dal.Models.Account ToAccountDB()
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
