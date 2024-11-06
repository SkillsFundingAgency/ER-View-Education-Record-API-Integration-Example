using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VERAExample.Models;

namespace VERAExample.Controllers
{
    public abstract class AbstractBaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected AbstractBaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected async Task<T> GetDataAsync<T>(string url) where T : class
        {
            T output = null;
            var result = await GetDataAsync(url);

            if (result is {IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK})
            {
                string content = await result.Content.ReadAsStringAsync();
                output = JsonSerializer.Deserialize<T>(content);
            }
            return output;
        }

        protected async Task<HttpResponseMessage> GetDataAsync(string url)
        {
            using var client = await GetClientAsync();

            var result = await client.GetAsync(url);
            return result;
        }

        protected async Task<HttpResponseMessage> PostDataAsync<T>(string url, T data)
        {
            using var client = await GetClientAsync();

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(url, content);
            return result;
        }

        protected async Task<HttpClient> GetClientAsync()
        {
            var client = new HttpClient();
            var token = await GetBearerTokenAsync();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            client.BaseAddress = new Uri(_configuration.GetValue<string>("BaseAPIURL"));
            return client;
        }

        protected async Task<string> GetBearerTokenAsync()
        {
            // AAD - DfE Tenant that will issue the JWT
            // client id - the unique registration id that has been issued for the provider - it is important to get this correct. The UKPRN is bound behind the scenes
            // DO NOT share this value across multiple providers - each provider must have a unique value
            // client secret - the unique secret that has been issued by the DfE for the provider. The secret and the client id travel as a pair
            // resource (the API that you want permission for) (this will be supplied by DfE)

            var tenantId = _configuration.GetSection("DfESecuritySettings")["tenantId"];
            var clientId = _configuration.GetSection("DfESecuritySettings")["clientId"];
            var secret = _configuration.GetSection("DfESecuritySettings")["secret"];
            var resource = _configuration.GetSection("DfESecuritySettings")["resource"];
            var requestUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

            using var httpClient = new HttpClient();

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", secret },
                { "resource", resource }
            };

            var requestBody = new FormUrlEncodedContent(dict);
            var response = await httpClient.PostAsync(requestUrl, requestBody);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var aadToken = JsonSerializer.Deserialize<AzureADToken>(responseContent);
            return aadToken.AccessToken;
        }
    }
}
