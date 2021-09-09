using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Authentication
    {
        public Authentication()
        {
            ApplicationAuthentications = new HashSet<ApplicationAuthentication>();
            Authorizations = new HashSet<Authorization>();
        }

        public int Id { get; set; }
        public string UserAgent { get; set; }
        public string Ipadress { get; set; }
        public string Token { get; set; }
        public int LoginFirstStep { get; set; }
        public DateTime Date { get; set; }
        public bool IsValid { get; set; }

        public virtual Ip IpadressNavigation { get; set; }
        public virtual LoginFirstStep LoginFirstStepNavigation { get; set; }
        public virtual ICollection<ApplicationAuthentication> ApplicationAuthentications { get; set; }
        public virtual ICollection<Authorization> Authorizations { get; set; }
    }
}
