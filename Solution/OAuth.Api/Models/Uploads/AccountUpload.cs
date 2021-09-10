﻿using OAuth.Api.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using static OAuth.Api.Controllers.LoginController;

namespace OAuth.Api.Models.Uploads
{
    public class AccountUpload
    {
        private const int DefaultIconID = 1;

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string UserName { get { return _username; } set { _username = value.ToLowerInvariant(); } }
        private string _username;

        [Required]
        [Password]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; }
        
        [ZipCode]
        [StringLength(10, MinimumLength = 8)]
        public string ZipCode { get; set; }
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
            ZipCode = ZipCode,
            CreateDate = DateTime.UtcNow,
            Key = GenerateToken(LargerTokenSize),
            Email = Email,
            IsCompany = IsCompany.Value,
            Valid = false,
            ProfileImageId = DefaultIconID,
            PhoneNumber = PhoneNumber,
            Name = Name
        };

    }
}
