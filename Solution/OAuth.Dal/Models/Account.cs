using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountConfirmations = new HashSet<AccountConfirmation>();
            Applications = new HashSet<Application>();
            LoginFirstSteps = new HashSet<LoginFirstStep>();
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany { get; set; }
        public DateTime AcceptTermsDate { get; set; }
        public bool Valid { get; set; }
        public DateTime CreateDate { get; set; }
        public string ZipCode { get; set; }
        public int ProfileImageId { get; set; }

        public virtual Image ProfileImage { get; set; }
        public virtual Personal Personal { get; set; }
        public virtual ICollection<AccountConfirmation> AccountConfirmations { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<LoginFirstStep> LoginFirstSteps { get; set; }
    }
}
