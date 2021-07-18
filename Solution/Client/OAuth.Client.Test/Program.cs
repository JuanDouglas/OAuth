using OAuth.Client.Models.Enums;
using OAuth.Client.Models.Results;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OAuth.Client.Test
{
    class Program
    {
        public const string AppKey = "Stock-Manager-API";
        static void Main(string[] args)
        {
            Authentication authentication = new("Test Console", "JuanDouglas", "Am4@0309");
            Console.WriteLine($"\t** Nexus Authentication **\n{authentication}\n");

            NexusOAuth nexusOAuth = new(authentication);

            AuthorizationResult authorizationResult = nexusOAuth.GetAuthorization(AppKey, AuthorizationLevel.Basic);
            Console.WriteLine($"\t** Authorization **\n{authorizationResult}\n");

            ApiAuthentication apiAuthentication = new(AuthorizationLevel.Basic, AppKey, authentication);
            Console.WriteLine($"\t** Api Authentication **\n{apiAuthentication}\n");

            
            Console.WriteLine($"\t** Validation **\n{nexusOAuth.ValidLogin(apiAuthentication)}\n");

            AccountResult account = nexusOAuth.GetAccount(apiAuthentication);
            Console.WriteLine($"\t** Account **\n{account}\n");
            Console.ReadLine();
        }
 
    }
}
