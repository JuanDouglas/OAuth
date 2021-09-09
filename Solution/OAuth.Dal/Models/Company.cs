using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public int Icon { get; set; }
        public int Account { get; set; }

        public virtual Account AccountNavigation { get; set; }
        public virtual Image IconNavigation { get; set; }
    }
}
