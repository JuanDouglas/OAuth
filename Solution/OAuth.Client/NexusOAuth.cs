﻿using Newtonsoft.Json;
using OAuth.Client;
using OAuth.Client.Models.Enums;
using OAuth.Client.Models.Results;
using OAuth.Client.Models.Upload;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client
{
    public class NexusOAuth
    {
        public Authentication Authentication { get; set; }
        public NexusOAuth(Authentication authentication)
        {
            if (!authentication.Logued)
            {
                throw new ArgumentException("Você deve está logado, para iniciar um OAuth");
            }
            Authentication = authentication;
        }

        public async Task<AccountResult> GetAccountAsync(int account_id, string authorization_token, string app_key)
        {
            HttpRequestMessage httpRequestMessage = Authentication.AuthenticatedRequest;
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri($"{Authentication.Host}/OAuth/Account?app_key={app_key}&account_id={account_id}&auth_token={authorization_token}");

            var response = await Authentication.httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArgumentException("Invalid Login keys! Please log in again! ");
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {

            }
            string responseString = await response.Content.ReadAsStringAsync();

            AccountResult accountResult = JsonConvert.DeserializeObject<AccountResult>(responseString);

            if (accountResult.Equals(new AccountResult()) ||
                accountResult is null ||
                accountResult == null)
            {
                return null;
            }



            return accountResult;
        }

        public AccountResult GetAccount(int account_id, string authorization_token, string app_key)
        {
            Task<AccountResult> task = GetAccountAsync(account_id, authorization_token, app_key);
            task.Wait();
            return task.Result;
        }

        public async Task<bool> ValidLoginAsync(string authentication_token, string authorization_token, int account_id, string app_key)
        {
            HttpRequestMessage httpRequestMessage = Authentication.AuthenticatedRequest;
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri($"{Authentication.Host}/OAuth/Valid?app_key={app_key}&account_id={account_id}&authorization_token={authorization_token}&authentication_token={authentication_token}");

            var response = await Authentication.httpClient.SendAsync(httpRequestMessage);
            return response.IsSuccessStatusCode;
        }
        public bool ValidLogin(string authentication_token, string authorization_token, int account_id, string app_key)
        {
            Task<bool> task = ValidLoginAsync(authentication_token, authorization_token, account_id, app_key);
            task.Wait();
            return task.Result;
        }
        public async Task<AuthorizationResult> AuthorizeAsync(string app_key, AuthorizationLevel level)
        {
            HttpRequestMessage httpRequestMessage = Authentication.AuthenticatedRequest;
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri($"{Authentication.Host}/OAuth/Authorize?app_key={app_key}&level={(int)level}&redirect=false");

            HttpResponseMessage response = await Authentication.httpClient.SendAsync(httpRequestMessage);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArgumentException("Invalid Login keys! Please log in again! ");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Invalid App Key!");
            }

            string responseString = await response.Content.ReadAsStringAsync();
            AuthorizationResult authorizationResult = JsonConvert.DeserializeObject<AuthorizationResult>(responseString);

            response = await new HttpClient().GetAsync(authorizationResult.Redirect);
            responseString = await response.Content.ReadAsStringAsync();

            return authorizationResult;
        }
        public AuthorizationResult Authorize(string app_key, AuthorizationLevel level)
        {
            Task<AuthorizationResult> task = AuthorizeAsync(app_key, level);
            task.Wait();
            return task.Result;
        }
        public ValidLoginResult ValidLogin(ApiAuthentication apiAuthentication) {
            Task<ValidLoginResult> validLoginTask = ValidLoginAsync(apiAuthentication);
            validLoginTask.Wait();

            return validLoginTask.Result;
        }
        public async Task<ValidLoginResult> ValidLoginAsync(ApiAuthentication apiAuthentication) 
        {
            HttpRequestMessage requestMessage = Authentication.AuthenticatedRequest;
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = new Uri($"{Authentication.Host}/OAuth/ValidAuthentication?app_key={apiAuthentication.AppKey}");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new LoginApp(apiAuthentication)),Encoding.UTF8,"application/json");

            HttpResponseMessage responseMessage = await Authentication.httpClient.SendAsync(requestMessage);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ValidLoginResult>(responseString);
        }
        public async Task<ApplicationLoginResult> LoginAsync(AuthorizationResult authorizationResult)
        {
            HttpRequestMessage httpRequestMessage = Authentication.AuthenticatedRequest;
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri($"{Authentication.Host}/OAuth/AppAuthentication?app_key={authorizationResult.Application.Key}&authorization_token={authorizationResult.Key}&account_id={authorizationResult.AccountID}");

            HttpResponseMessage response = await Authentication.httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArgumentException("Invalid Login keys! Please log in again! ");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Invalid App Key!");
            }
            string responseString = await response.Content.ReadAsStringAsync();

            ApplicationLoginResult loginResult = JsonConvert.DeserializeObject<ApplicationLoginResult>(responseString);

            response = await new HttpClient().GetAsync(authorizationResult.Redirect);
            responseString = await response.Content.ReadAsStringAsync();

            return loginResult;
        }

        public ApplicationLoginResult Login(AuthorizationResult authorization)
        {
            Task<ApplicationLoginResult> task = LoginAsync(authorization);
            task.Wait();
            return task.Result;
        }
    }
}
