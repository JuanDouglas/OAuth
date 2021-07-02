using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Dal.Models
{
    public class Account
    {
        public Account()
        {
            Applications = new HashSet<Application>();
            LoginFirstSteps = new HashSet<LoginFirstStep>();
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsCompany { get; set; }
        public DateTime AcceptTermsDate { get; set; }
        public bool Valid { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProfileImageId { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Image ProfileImage { get; set; }
        public virtual AccountDetail AccountDetail { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<LoginFirstStep> LoginFirstSteps { get; set; }
    }
}
