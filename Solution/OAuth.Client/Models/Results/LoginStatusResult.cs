using System;

namespace OAuth.Client.Models.Results
{
    public class LoginStatusResult
    {
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public string UserAgent { get; set; }
        public IPAdressResult IP { get; set; }
        public string AccountKey { get; set; }

        public LoginStatusResult()
        {
        }
    }
}
