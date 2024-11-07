using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using VERAExample.Models;


namespace VERAExample.Controllers
{
    public class HomeController : AbstractBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private RetrieveModel model;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
            : base(config)
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
        public async Task<IActionResult> QrCode()
        {
            var result = await GetDataAsync<QrCodeModel>("generate-qr-code");
            return View(result);
        }

        public async Task<IActionResult> LearnerData(string id)
        {
            var result = await GetDataAsync<LearnerData>("learner-data?correlationId={id}");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Retrieve(RetrieveModel model)
        {
            var result = await GetDataAsync<LearnerData>($"learner-data?uln={model.ULN}");
            return View(result);
        }

    }
}
