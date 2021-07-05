using Newtonsoft.Json;
using OAuth.Client.Exceptions;
using OAuth.Client.Models.Results;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth.Client
{
    /// <summary>
    /// Authentication in OAuth Server.
    /// </summary>
    public class Authentication
    {
        public string AccountKey { get; set; }
        public string AuthenticationToken { get; set; }
        public string FirstStepKey { get; set; }
        public string UserAgent { get; set; }
        public bool Logued { get; private set; }
        public DateTime Date { get; set; }
        public string IPAdress { get; set; }
        internal HttpRequestMessage AuthenticatedRequest
        {
            get
            {
                if (!Logued)
                {
                    throw new LoginException("You must be logged in to obtain a request that requires authentication.");
                }
                HttpRequestMessage httpRequestMessage = new();
                httpRequestMessage.Headers.Add(AccountKeyHeader, AccountKey);
                httpRequestMessage.Headers.Add(AuthenticationTokenHeader, AuthenticationToken);
                httpRequestMessage.Headers.Add(FirstStepKeyHeader, FirstStepKey);
                httpRequestMessage.Headers.Add("User-Agent", UserAgent);
                return httpRequestMessage;
            }
        }
        internal static string Host = "https://nexus-oauth.azurewebsites.net/api";
        internal static readonly HttpClient httpClient = new(new HttpClientHandler()
        {
            AllowAutoRedirect = false
            //ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyerrors) =>
            //{
            //    return true;
            //}
        })
        ;
        public const string AuthenticationTokenHeader = "Authentication-Token";
        public const string AccountKeyHeader = "Account-Key";
        public const string FirstStepKeyHeader = "First-Step-Key";
        /// <summary>
        /// Constructor for User-Agent
        /// </summary>
        /// <param name="user_agent">Name for you HTTP 'User-Agent'.</param>
        public Authentication(string user_agent)
        {
            AccountKey = string.Empty;
            AuthenticationToken = string.Empty;
            FirstStepKey = string.Empty;
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
        public void Login(string user, string pwd)
        {
            try
            {
                Task task = LoginAsync(user, pwd);
                task.Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is LoginException)
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
            HttpRequestMessage requestMessage = new(HttpMethod.Get,
                $"{Host}/Login/FirstStep?user={user}&web_page=false&redirect=none");

            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LoginException(LoginException.UserField, "The user does not exist or is not typed correctly!");
            }

            string responseString = await responseMessage.Content.ReadAsStringAsync();

            FirstStepResult loginFirstStep = JsonConvert.DeserializeObject<FirstStepResult>(responseString);
            requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{Host}/Login/SecondStep?pwd={pwd}&key={loginFirstStep.Key}&web_page=false&redirect=none&fs_id={loginFirstStep.ID}");
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

            responseString = await responseMessage.Content.ReadAsStringAsync();
            AuthenticationResult authenticationResult = JsonConvert.DeserializeObject<AuthenticationResult>(responseString);

            AccountKey = authenticationResult.AccountKey;
            AuthenticationToken = authenticationResult.Token;
            FirstStepKey = loginFirstStep.Key;
            Logued = authenticationResult.IsValid;
            Date = authenticationResult.Date.ToLocalTime();
            IPAdress = authenticationResult.IPAdress;
        }

        public override string ToString()
        {
            return $"Account Key: {AccountKey}\nFirst step Key: {FirstStepKey}\nAuthentication Token: {AuthenticationToken}\nUser Agent: {UserAgent}\nIP Adress: {IPAdress}\nDate: {Date}";
        }
    }
}
