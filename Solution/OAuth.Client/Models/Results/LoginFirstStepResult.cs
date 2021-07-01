using System;

namespace OAuth.Client.Models.Results
{
    public class LoginFirstStepResult
    {
        /// <summary>
        /// Date for First Step.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// First Step Token.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// IP for original request.
        /// </summary>
        public IPAdressResult IP { get; set; }
        /// <summary>
        /// Indicates whether the token is still valid.
        /// </summary>
        public bool IsValid { get; set; }

        public LoginFirstStepResult()
        {
        }
    }
}
