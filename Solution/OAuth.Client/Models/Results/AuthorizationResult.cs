using System;

namespace OAuth.Client.Models.Results
{
    public class AuthorizationResult
    {
        public int ID { get; set; }
        public ApplicationResult Application { get; set; }
        public int Level { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
        public string Token { get; set; }
        public int AccountID { get; set; }
        public string Redirect { get; set; }
        public AuthorizationResult()
        {

        }
    }
}