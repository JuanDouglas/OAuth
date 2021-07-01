using System;
using System.Collections.Generic;

namespace OAuth.Client.Models.Results
{
    /// <summary>
    /// Account for OAuth API Search.
    /// </summary>
    public class AccountResult
    { /// <summary>
      /// Account ID.
      /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Username for Login.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Account Create Date.
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// E-mail.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Valided E-mail.
        /// </summary>
        public bool IsValidEmail { get; set; }
        /// <summary>
        /// Is Company Account.
        /// </summary>
        public bool IsCompany { get; set; }
        /// <summary>
        /// Profile Image.
        /// </summary>
        public FileModel ProfileImage { get; set; }
        public List<AuthorizationResult> Authorizations { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}\nUserName: {UserName}\nEmail: {Email}\nIsCompany: {IsCompany}";
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

                if (@object.IsValidEmail != IsValidEmail)
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
