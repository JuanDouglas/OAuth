using System;
using System.Collections.Generic;

namespace OAuth.Client.Models.Results
{
    /// <summary>
    /// Account for OAuth API Search.
    /// </summary>
    public class AccountResult
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany { get; set; }
        public DateTime AcceptTermsDate { get; set; }
        public bool Valid { get; set; }
        public DateTime CreateDate { get; set; }
        public FileModel ProfileImage { get; set; }
        public List<AuthorizationResult> Authorizations { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}\nEmail: {Email}\nPhoneNumber: {PhoneNumber}\nAcceptTermsDate: {AcceptTermsDate}\nCreate Account Date: {CreateDate}\nIsCompany: {IsCompany}\nValided: {Valid}";
        }
        /// <summary>
        /// Compare obj for equal result.
        /// </summary>
        /// <param name="obj">Object for camparation</param>
        /// <returns>resulkt</returns>
        public override bool Equals(object obj)
        {
            if (obj is AccountResult)
            {
                AccountResult @object = obj as AccountResult;

                if (@object.ID != ID)
                {
                    return false;
                }

                if (@object.Email != Email)
                {
                    return false;
                }

                if (@object.CreateDate != CreateDate)
                {
                    return false;
                }

                if (@object.IsCompany != IsCompany)
                {
                    return false;
                }



                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
