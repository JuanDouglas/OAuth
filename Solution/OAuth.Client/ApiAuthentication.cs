using OAuth.Client.Models.Enums;
using OAuth.Client.Models.Results;
using System.Threading.Tasks;

namespace OAuth.Client
{
    /// <summary>
    /// Authentication in APi using OAuth Api.
    /// </summary>
    public class ApiAuthentication
    {
        public string Authentication { get; set; }
        public string Authorization { get; set; }
        public int AccountID { get; set; }
        public static string UserAgent { get; set; }
        public string AppKey { get; set; }
        public Level Level { get; set; }
        private Authentication userAuthentication;

        public ApiAuthentication()
        {

        }
        public ApiAuthentication(string appKey)
        {
            AppKey = appKey;
            Level = Level.Basic;
        }
        public ApiAuthentication(string appKey, Level level) : this(appKey)
        {
            Level = level;
        }
        public ApiAuthentication(Level level, string app_key, Authentication authentication)
        {
            UserAgent = authentication.UserAgent;
            userAuthentication = authentication;
            AppKey = app_key;
            Level = level;
            Login();
        }
        public ApiAuthentication(string app_key, string userAgent, string user, string pwd) : this(Level.Basic, app_key, userAgent, user, pwd)
        {

        }
        public ApiAuthentication(Level level, string app_key, string userAgent, string user, string pwd) : this(level, app_key, new Authentication(userAgent, user, pwd))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationLoginResult> LoginAsync()
        {
            NexusOAuth nexusOAuth = new NexusOAuth(userAuthentication);
            AuthorizationResult authorizationResult = await nexusOAuth.AuthorizeAsync(AppKey, Level);
            ApplicationLoginResult applicationLogin = await nexusOAuth.LoginAsync(authorizationResult);

            Authentication = applicationLogin.Token;
            Authorization = authorizationResult.Token;
            AccountID = authorizationResult.AccountID;
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
            return $"Authentication: {Authentication}\nAuthorization: {Authorization}\nAccount ID: {AccountID}\nUser-Agent: {UserAgent}\nAppKey: {AppKey}";
        }
    }
}
