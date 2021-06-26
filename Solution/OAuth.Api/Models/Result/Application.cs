using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Result
{
    public class Application
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public File Icon { get; set; }
        public Application()
        {

        }
        public Application(Dal.Models.Application application)
        {
            Name = application.Name;
            Key = application.Key;
            Icon = new(application.IconNavigation);
        }
    }
}
