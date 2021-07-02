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
        public const int DefaultIconID = 2;
        [Required]
        [StringLength(500)]
        [ContainsString(new[] { OAuthController.ReplaceAuthorizationToken, OAuthController.ReplaceAccountID, OAuthController.ReplaceAuthenticationToken })]
        public string LoginRedirect { get; set; }
        [Required]
        [StringLength(500)]
        [ContainsString(new[] { OAuthController.ReplaceAuthorizationToken, OAuthController.ReplaceAccountID })]
        public string AuthorizeRedirect { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Site { get; set; }

        public ApplicationUpload()
        {

        }
        public Dal.Models.Application ToApplicationDB() => new()
        {
            PrivateKey = LoginController.GenerateToken(LoginController.NormalTokenSize),
            Key = LoginController.GenerateToken(LoginController.NormalTokenSize),
            AuthorizeRedirect = AuthorizeRedirect,
            LoginRedirect = LoginRedirect,
            Icon = DefaultIconID,
            Site = Site,
            Name = Name
        };
    }
}
