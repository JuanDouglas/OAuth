using System;

namespace OAuth.Client.Models.Results
{
    public class AuthenticationResult
    {
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public bool IsValid { get; set; }
        public string AccountKey { get; set; }
        public AuthenticationResult()
        {
        }
    }
}
