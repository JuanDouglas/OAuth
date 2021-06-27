using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationAuthentications = new HashSet<ApplicationAuthentication>();
            Authorizations = new HashSet<Authorization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string AuthorizeRedirect { get; set; }
        public string LoginRedirect { get; set; }
        public string Site { get; set; }
        public int Owner { get; set; }
        public int Icon { get; set; }
        public string PrivateKey { get; set; }

        public virtual Image IconNavigation { get; set; }
        public virtual Account OwnerNavigation { get; set; }
        public virtual ICollection<ApplicationAuthentication> ApplicationAuthentications { get; set; }
        public virtual ICollection<Authorization> Authorizations { get; set; }
    }
}
