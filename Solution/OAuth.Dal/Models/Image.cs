using System;
using System.Collections.Generic;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class Image
    {
        public Image()
        {
            Accounts = new HashSet<Account>();
            Applications = new HashSet<Application>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public int FileType { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}
