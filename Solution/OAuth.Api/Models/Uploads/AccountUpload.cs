using OAuth.Api.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using static OAuth.Api.Controllers.LoginController;

namespace OAuth.Api.Models.Uploads
{
    public class AccountUpload
    {
        private const int DefaultIconID = 1;
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
        [Phone]
        [Required]
        [StringLength(32)]
        public string PhoneNumber { get; set; }
        public Nullable<bool> IsCompany { get; set; }
        [Required]
        public bool AcceptTerms { get; set; }

        public AccountUpload()
        {
        }

        public Dal.Models.Account ToAccountDB() => new()
        {
            Password = HashPassword(Password),
            AcceptTermsDate = DateTime.UtcNow,
            UserName = UserName,
            CreateDate = DateTime.UtcNow,
            Key = GenerateToken(LargerTokenSize),
            Email = Email,
            IsCompany = IsCompany,
            Valid = false,
            ProfileImageId = DefaultIconID
        };

    }
}
