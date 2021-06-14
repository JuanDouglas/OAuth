using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Authorization
    {
        public Authorization()
        {
            ApplicationAuthentications = new HashSet<ApplicationAuthentication>();
        }

        public int Id { get; set; }
        public int Authentication { get; set; }
        public int Application { get; set; }
        public string Key { get; set; }
        public bool Active { get; set; }
        public int Level { get; set; }
        public DateTime Date { get; set; }

        public virtual Application ApplicationNavigation { get; set; }
        public virtual Authentication AuthenticationNavigation { get; set; }
        public virtual ICollection<ApplicationAuthentication> ApplicationAuthentications { get; set; }
    }
}
