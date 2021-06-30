using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Ip
    {
        public Ip()
        {
            ApplicationAuthentications = new HashSet<ApplicationAuthentication>();
            Authentications = new HashSet<Authentication>();
            FailAttemps = new HashSet<FailAttemp>();
            LoginFirstSteps = new HashSet<LoginFirstStep>();
        }

        public int Id { get; set; }
        public string Adress { get; set; }
        public int Confiance { get; set; }
        public bool AlreadyBeenBanned { get; set; }

        public virtual ICollection<ApplicationAuthentication> ApplicationAuthentications { get; set; }
        public virtual ICollection<Authentication> Authentications { get; set; }
        public virtual ICollection<FailAttemp> FailAttemps { get; set; }
        public virtual ICollection<LoginFirstStep> LoginFirstSteps { get; set; }
    }
}
