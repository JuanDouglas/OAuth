using OAuth.Client.Models.Enums;
using System;

namespace OAuth.Client.Models.Results
{
    public class AuthorizationResult
    {
        public ApplicationResult Application { get; set; }
        public string Key { get; set; }
        public bool Active { get; set; }
        public AuthorizationLevel Level { get; set; }
        public DateTime Date { get; set; }
        public int AccountID { get; set; }
        public string Redirect { get; set; }
        public AuthorizationResult()
        {

        }
    }
}