using OAuth.Dal;
using OAuth.Dal.Models;
using System.Linq;

namespace OAuth.Api.Models.Result
{
    public class Application
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Site { get; set; }
        public File Icon { get; set; }
        private OAuthContext db = new();
        public Application()
        {

        }
        public Application(Dal.Models.Application application)
        {
            Dal.Models.Image icon = db.Images.FirstOrDefault(fs => fs.Id == application.Icon);
            Name = application.Name;
            Key = application.Key;
            Site = application.Site;
            Icon = new(icon);
        }
    }
}
