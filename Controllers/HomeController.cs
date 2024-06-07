using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using VERAExample.Models;


namespace VERAExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private RetrieveModel model;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            model = new RetrieveModel();
        }

        public IActionResult Index()
        {
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult QrCode()
        {
            HttpClient client = new HttpClient();
            string token = GetBearerToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var result = client.GetAsync("https://api.tst.view-education-record.education.gov.uk/api/v1/education-records/generate-qr-code").Result;
            QrCodeModel qrCode = null;
            if (result.IsSuccessStatusCode)
            {
                string content = result.Content.ReadAsStringAsync().Result;
                qrCode = JsonSerializer.Deserialize<QrCodeModel>(content);
            }
            return View(qrCode);
        }

        private string GetBearerToken()
        {
            // AAD - DfE Tenant that will issue the JWT
            // client id - the unique registration id that has been issued for the provider - it is important to get this correct. The UKPRN is bound behind the scenes
            // client secret - the unique secret that has been issued by the DfE for the provider.
            // resource (the API that you want permission for)

            var tenantId = _configuration.GetSection("DfESecuritySettings")["tenantId"];
            var clientId = _configuration.GetSection("DfESecuritySettings")["clientId"];
            var secret = _configuration.GetSection("DfESecuritySettings")["secret"];
            var resource = _configuration.GetSection("DfESecuritySettings")["resource"];
            var requestUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

            var httpClient = new HttpClient();

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", secret },
                { "resource", resource }
            };

            var requestBody = new FormUrlEncodedContent(dict);
            var response = httpClient.PostAsync(requestUrl, requestBody).Result;

            response.EnsureSuccessStatusCode();

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var aadToken = JsonSerializer.Deserialize<AzureADToken>(responseContent);
            return aadToken.AccessToken;
        }

        public IActionResult LearnerData(string id)
        {
            HttpClient client = new HttpClient();
            string token = GetBearerToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var result = client.GetAsync($"https://api.tst.view-education-record.education.gov.uk/api/v1/education-records/learner-data?correlationId={id}").Result;
            LearnerData data = null;
            if (result.IsSuccessStatusCode)
            {
                string content = result.Content.ReadAsStringAsync().Result;
                data = JsonSerializer.Deserialize<LearnerData>(content);
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult Retrieve(RetrieveModel model)
        {
            HttpClient client = new HttpClient();
            string token = GetBearerToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var result = client.GetAsync($"https://api.tst.view-education-record.education.gov.uk/api/v1/education-records/learner-data?uln={model.ULN}").Result;
            LearnerData data = null;
            if (result.IsSuccessStatusCode)
            {
                string content = result.Content.ReadAsStringAsync().Result;
                data = JsonSerializer.Deserialize<LearnerData>(content);
            }
            return View(data);
        }

    }
}
