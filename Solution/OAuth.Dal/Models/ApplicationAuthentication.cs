using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class ApplicationAuthentication
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public string Ipadress { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public int Application { get; set; }
        public int Authorization { get; set; }
        public int Authentication { get; set; }

        public virtual Application ApplicationNavigation { get; set; }
        public virtual Authentication AuthenticationNavigation { get; set; }
        public virtual Authorization AuthorizationNavigation { get; set; }
        public virtual Ip IpadressNavigation { get; set; }
    }
}
