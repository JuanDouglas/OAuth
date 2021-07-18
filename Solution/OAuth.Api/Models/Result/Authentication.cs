using System;

namespace OAuth.Api.Models.Result
{
    public class Authentication
    {
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public bool ValidedAccount { get; set; }
        public string AccountKey { get; set; }
        public Authentication() { }
        public Authentication(Dal.Models.Authentication authentication)
        {
            UserAgent = authentication.UserAgent;
            IPAdress = authentication.Ipadress;
            Token = authentication.Token;
            Date = authentication.Date;
        }
    }
}
