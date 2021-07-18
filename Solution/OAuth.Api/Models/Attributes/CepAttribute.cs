using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CepAttribute : ValidationAttribute
    {
        public CepAttribute()
        {
            ErrorMessage = "Incorrect or invalid!";
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                HttpClient httpClient = new();
                Task<HttpResponseMessage> response = httpClient.GetAsync($"https://open-cep.azurewebsites.net/api/Cep/ByNumber?number={Regex.Replace(value.ToString(), @"[^0-9]+", string.Empty)}");
                response.Wait();
                if (response.Result.StatusCode != HttpStatusCode.OK)
                    return false;

                return true;
            }
            return base.IsValid(value);
        }
    }
}