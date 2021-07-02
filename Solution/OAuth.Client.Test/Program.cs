using OAuth.Client.Models.Enums;
using System;
using System.Threading.Tasks;

namespace OAuth.Client.Test
{
    class Program
    {
        public const string AppKey = "Stock-Manager-API";
        static void Main(string[] args)
        {
            Authentication authentication = new("Test Console", "JuanDouglas", "Am4@0309");
            Console.WriteLine($"\t**Nexus Authentication*** \n\n{authentication}\n");

            ApiAuthentication apiAuthentication = new(AuthorizationLevel.Basic, AppKey, authentication);
            Console.WriteLine($"\t**Api Authentication*** \n\n{apiAuthentication}\n");
            Console.ReadLine();
        }
    }
}
