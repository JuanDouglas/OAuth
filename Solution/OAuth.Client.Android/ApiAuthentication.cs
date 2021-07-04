using OAuth.Client.Android.Exceptions;
using OAuth.Client.Android.Models.Enums;
using OAuth.Client.Android.Models.Results;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth.Client.Android
{
    /// <summary>
    /// Authentication in API using OAuth Api.
    /// </summary>
    public class ApiAuthentication
    {
        /// <summary>
        /// AuthorizationToken
        /// </summary>
        public string AuthorizationToken { get; set; }
        /// <summary>
        /// User account identification.
        /// </summary>
        public int AccountID { get; set; }
        /// <summary>
        /// Token of authentication
        /// </summary>
        public string AuthenticationToken { get; set; }
        /// <summary>
        /// Client user agent
        /// </summary>
        public static string UserAgent { get; set; }
        /// <summary>
        /// Applicatipon unqiue key.
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// Authorization level.
        /// </summary>
        public AuthorizationLevel Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Authenticated { get { return oAuth.ValidLogin(this).IsValid; } }
        public HttpRequestMessage AuthenticateRequest
        {
            get
            {
                if (!Authenticated)
                {
                    throw new LoginException("You must be authenticated in to obtain a request that requires authentication.");
                }
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add(AccountIDHeader, AccountID.ToString());
                httpRequestMessage.Headers.Add(AuthenticationTokenHeader, AuthenticationToken);
                httpRequestMessage.Headers.Add(AuthorizationTokenHeader, AuthorizationToken);
                httpRequestMessage.Headers.Add("User-Agent", UserAgent);
                return httpRequestMessage;
            }
        }
        public const string AccountIDHeader = "AccountID";
        public const string AuthenticationTokenHeader = "AuthenticationToken";
        public const string AuthorizationTokenHeader = "AuthorizationToken";
        private NexusOAuth oAuth;
        private AuthorizationResult authorization;
        private ApplicationLoginResult applicationLogin;
        private Authentication userAuthentication;

        public ApiAuthentication()
        {

        }
        public ApiAuthentication(string appKey)
        {
            AppKey = appKey;
            Level = AuthorizationLevel.Basic;
        }
        public ApiAuthentication(string appKey, AuthorizationLevel level) : this(appKey)
        {
            Level = level;
        }
        public ApiAuthentication(AuthorizationLevel level, string app_key, Authentication authentication)
        {
            UserAgent = authentication.UserAgent;
            userAuthentication = authentication;
            AppKey = app_key;
            Level = level;
            Login();
        }
        public ApiAuthentication(string app_key, string userAgent, string user, string pwd) : this(AuthorizationLevel.Basic, app_key, userAgent, user, pwd)
        {

        }
        public ApiAuthentication(AuthorizationLevel level, string app_key, string userAgent, string user, string pwd) : this(level, app_key, new Authentication(userAgent, user, pwd))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationLoginResult> LoginAsync()
        {
            oAuth = new NexusOAuth(userAuthentication);

            try
            {
                authorization = await oAuth.GetAuthorizationAsync(AppKey, Level);
            }
            catch (AuthorizationNotFoundException)
            {
                authorization = await oAuth.AuthorizeAsync(AppKey, Level);
            }

            applicationLogin = await oAuth.LoginAsync(authorization);

            AuthorizationToken = applicationLogin.AuthorizationToken;
            AccountID = authorization.AccountID;
            AuthenticationToken = applicationLogin.LoginToken;

            return applicationLogin;
        }

        public ApplicationLoginResult Login()
        {
            Task<ApplicationLoginResult> task = LoginAsync();
            task.Wait();
            return task.Result;
        }

        public override string ToString()
        {
            return $"Authentication: {AuthenticationToken}\nAuthorization: {AuthorizationToken}\nAccount ID: {AccountID}\nUser-Agent: {UserAgent}\nAppKey: {AppKey}";
        }
    }
}
