using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Result
{
    public class ApplicationOwner
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Site { get; set; }
        public string PrivateKey { get; set; }
        public File Icon { get; set; }

        public ApplicationOwner()
        {

        }
        public ApplicationOwner(Dal.Models.Application application)
        {
            Name = application.Name;
            Key = application.Key;
            Site = application.Site;
            Icon = new(application.IconNavigation);
            PrivateKey = application.PrivateKey;
        }
    }
}
