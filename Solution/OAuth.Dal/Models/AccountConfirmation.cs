using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class AccountConfirmation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public int Account { get; set; }

        public virtual Account AccountNavigation { get; set; }
    }
}
