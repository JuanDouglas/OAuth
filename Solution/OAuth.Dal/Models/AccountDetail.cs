using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class AccountDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CpforCnpj { get; set; }
        public int Account { get; set; }

        public virtual Account AccountNavigation { get; set; }
    }
}
