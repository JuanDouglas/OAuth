using OAuth.Api.Controllers;
using OAuth.Api.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Uploads
{
    public class ApplicationUpload
    {
        [Required]
        [ContainsString(new[] { OAuthController.ReplaceAuthorizationToken, OAuthController.ReplaceAccountID, OAuthController.ReplaceAuthenticationToken })]
        public string LoginRedirect { get; set; }

        [Required]
        [ContainsString(new[] { OAuthController.ReplaceAuthorizationToken, OAuthController.ReplaceAccountID})]
        public string AuthorizeRedirect { get; set; }

    }
}
