using System;

namespace OAuth.Client.Android.Models.Results
{
    public class ApplicationLoginResult
    {
        public IPAdressResult IPAdress { get; set; }
        public ApplicationResult ApplicationResult { get; set; }
        public string UserAgent { get; set; }
        public DateTime Date { get; set; }
        public string Token { get; set; }

        public ApplicationLoginResult()
        {
        }
    }
}
