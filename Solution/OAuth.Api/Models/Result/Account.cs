using OAuth.Dal;
using OAuth.Dal.Models;
using System;
using System.Linq;

namespace OAuth.Api.Models.Result
{
    public class Account
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany { get; set; }
        public DateTime AcceptTermsDate { get; set; }
        public bool Valid { get; set; }
        public DateTime CreateDate { get; set; }
        public File ProfileImage { get; set; }

        private readonly OAuthContext db = new();
        public Account()
        {
        }

        public Account(Dal.Models.Account account)
        {
            account.ProfileImage ??= db.Images.FirstOrDefault(fs => fs.Id == account.ProfileImageId);
            account.ProfileImage ??= new Dal.Models.Image();

            ID = account.Id;
            UserName = account.UserName;
            Email = account.Email;
            IsCompany = account.IsCompany;
            Key = account.Key;
            ProfileImage = new(account.ProfileImage);
            AcceptTermsDate = account.AcceptTermsDate;
            CreateDate = account.CreateDate;
            Valid = account.Valid;
            PhoneNumber = account.PhoneNumber;
        }

    }
}
