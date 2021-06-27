using System;

#nullable disable

namespace OAuth.Dal.Models
{
    public partial class FailAttemp
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Ipadress { get; set; }
        public int AttempType { get; set; }

        public virtual Ip IpadressNavigation { get; set; }
    }
}
