using System;

namespace OAuth.Api.Models.Result
{
    public class Authentication
    {
        public int ID { get; set; }
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
        public bool IsValid { get; set; }
        public string AccountKey { get; set; }
        public Authentication() { }
        public Authentication(Dal.Models.Authentication authentication)
        {
            ID = authentication.Id;
            UserAgent = authentication.UserAgent;
            IPAdress = authentication.Ipadress;
            Token = authentication.Token;
            Date = authentication.Date;
            IsValid = authentication.IsValid;
        }
    }
}
