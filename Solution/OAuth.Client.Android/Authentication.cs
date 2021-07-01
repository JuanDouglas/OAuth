using Newtonsoft.Json;
using OAuth.Client.Android.Exceptions;
using OAuth.Client.Android.Models.Results;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth.Client.Android
{
    /// <summary>
    /// Authentication in OAuth Server.
    /// </summary>
    public class Authentication
    {
        public string UserKey { get; set; }
        public string LoginToken { get; set; }
        public string UserAgent { get; set; }
        public bool Logued { get; private set; }
        internal HttpRequestMessage AuthenticatedRequest
        {
            get
            {
                if (!Logued)
                {
                    throw new LoginException("You must be logged in to obtain a request that requires authentication.");
                }
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add(AuthAcountKey, UserKey);
                httpRequestMessage.Headers.Add(AuthKey, LoginToken);
                httpRequestMessage.Headers.Add("User-Agent", UserAgent);
                return httpRequestMessage;
            }
        }
        internal static string Host = "https://nexus-oauth.azurewebsites.net/api";
        internal static readonly HttpClient httpClient = new HttpClient(new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyerrors) =>
            {
                return true;
            }
        });
        public const string AuthAcountKey = "auth-account-key";
        public const string AuthKey = "auth-token";
        /// <summary>
        /// Constructor for User-Agent
        /// </summary>
        /// <param name="user_agent">Name for you HTTP 'User-Agent'.</param>
        public Authentication(string user_agent)
        {
            UserKey = string.Empty;
            LoginToken = string.Empty;
            UserAgent = user_agent;
        }

        /// <summary>
        /// Constructor for User-Agent and execute Login.
        /// </summary>
        /// <param name="user">Username for login.</param>
        /// <param name="pwd">Password for login.</param>
        public Authentication(string user_agent, string user, string pwd) : this(user_agent)
        {
            Login(user, pwd);
        }
        /// <summary>
        /// Login in OAuth Server.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="pwd">Password</param>
        /// <returns>void</returns>
        public void Login(string user, string pwd)
        {
            try
            {
                Task task = LoginAsync(user, pwd);
                task.Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions is LoginException)
                {
                    LoginException exception = e.InnerException as LoginException;
                    throw exception;
                }
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Login in OAuth Server.
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="pwd">Password</param>
        /// <returns>Task for work.</returns>
        public async Task LoginAsync(string user, string pwd)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{Host}/OAuth/Login/FirstStep?user={user}&web_view=false&post=none");

            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LoginException(LoginException.UserField, "The user does not exist or is not typed correctly!");
            }

            LoginFirstStepResult loginFirstStep = JsonConvert.DeserializeObject<LoginFirstStepResult>(await responseMessage.Content.ReadAsStringAsync());
            requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{Host}/OAuth/Login/SecondStep?pwd={pwd}&key={loginFirstStep.Token}&web_view=false&post=none");
            requestMessage.Headers.Add("User-Agent", UserAgent);
            responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new LoginException(LoginException.PasswordField, "The password is not entered correctly or is wrong!");
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LoginException("One error ocurred!");
            }
            string stringResponse = await responseMessage.Content.ReadAsStringAsync();
            LoginStatusResult loginResult = JsonConvert.DeserializeObject<LoginStatusResult>(stringResponse);
            UserKey = loginResult.AccountKey;
            LoginToken = loginResult.Token;
            Logued = true;
        }


    }
}
