using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Enums
{
    public enum AttempType : uint
    {
        AuthorizeApp,
        /// <summary>
        /// Occurs when logging into an app does not result in success
        /// </summary>
        LoginInApp,
        /// <summary>
        /// Occurs when the password received by the server and different from the user's password
        /// </summary>
        PasswordIncorrect,
        /// <summary>
        /// Occurs when The FirstStepKey and Invalid
        /// </summary>
        FirsStepInvalid,
        /// <summary>
        /// Occurs when the user entered in the first step of login and invalidates
        /// </summary>
        UserInvalid,
        /// <summary>
        /// Occurs when login information is invalidated
        /// </summary>
        RequestLoginInvalid
    }
}
