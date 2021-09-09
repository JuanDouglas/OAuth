using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class LoginFirstStep
    {
        public LoginFirstStep()
        {
            Authentications = new HashSet<Authentication>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Account { get; set; }
        public string Token { get; set; }
        public bool Valid { get; set; }
        public string Ipadress { get; set; }

        public virtual Account AccountNavigation { get; set; }
        public virtual Ip IpadressNavigation { get; set; }
        public virtual ICollection<Authentication> Authentications { get; set; }
    }
}
