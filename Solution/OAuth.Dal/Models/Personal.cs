using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Personal
    {
        public int Id { get; set; }
        public int Account { get; set; }

        public virtual Account AccountNavigation { get; set; }
    }
}
