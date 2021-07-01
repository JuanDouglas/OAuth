using OAuth.Client.Models.Results;
using System;
using System.Threading.Tasks;

namespace OAuth.Client.Test
{
    class Program
    {
        public const string AppKey = "Stock-Manager-API";
        static async Task Main(string[] args)
        {
            Authentication authentication = new("Test Console", "Master", "Senha");
            NexusOAuth nexusOAuth = new(authentication);
            ApiAuthentication apiAuthentication = new("Stock-Manager-API", "Test Console", "Master", "Senha");

            AccountResult account = nexusOAuth.GetAccount(apiAuthentication.AccountID, apiAuthentication.Authorization, AppKey);
            Console.WriteLine(apiAuthentication.ToString());
            Console.ReadLine();
        }
    }
}
