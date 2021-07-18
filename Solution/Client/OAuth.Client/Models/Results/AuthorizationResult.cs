using OAuth.Client.Models.Enums;
using System;

namespace OAuth.Client.Models.Results
{
    public class AuthorizationResult
    {
        public ApplicationResult Application { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public AuthorizationLevel Level { get; set; }
        public DateTime Date { get; set; }
        public int AccountID { get; set; }
        public string Redirect { get; set; }
        public AuthorizationResult()
        {

        }
        public override string ToString()
        {
            return $"AccountID {AccountID}\nAuthorization Token: {Token}\nAuthorization Date: {Date}\nActive: {Active}\nRedirect URL: {Redirect}\nLevel: {Level}";
        }
    }
}