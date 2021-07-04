using System;

namespace OAuth.Client.Android.Models.Results
{
    public class ApplicationLoginResult
    {
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public DateTime Date { get; set; }
        public ApplicationResult Application { get; set; }
        public string AuthorizationToken { get; set; }
        public string LoginToken { get; set; }
        public int AccountID { get; set; }
        public string Redirect { get; set; }
        public ApplicationLoginResult()
        {
        }
    }
}
