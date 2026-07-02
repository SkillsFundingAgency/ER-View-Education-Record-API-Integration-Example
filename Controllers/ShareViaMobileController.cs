using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VERAExample.Models;

namespace VERAExample.Controllers
{
    public class ShareViaMobileController : AbstractBaseController
    {
        public ShareViaMobileController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Remove("MobileNumber");
            HttpContext.Session.Remove("DateOfBirth");
            return View();
        }


        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitForm(LearnerShareRequest request)
        {
            // Store values in session
            HttpContext.Session.SetString("MobileNumber", request.MobileNumber);
            HttpContext.Session.SetString("DateOfBirth", request.DateOfBirth.ToString("dd-MM-yyyy"));

            // Check if learner exists
            var learnerExists = await CheckIfLearnerExistsForEducationRecordAsync(request.MobileNumber, request.DateOfBirth);
            if (learnerExists)
            {
                // Redirect to a page to create a new learner
                return View("LearnerFound", request);
            }
            // Redirect to a confirmation page or back to the form
            return View("LearnerNotFound");
        }


        [HttpPost("Request")]
        public async Task<IActionResult> RequestLearnerShare(LearnerShareRequest request)
        {
            var mobileNumber = HttpContext.Session.GetString("MobileNumber");
            var dob = DateTime.ParseExact(HttpContext.Session.GetString("DateOfBirth"), "dd-MM-yyyy", CultureInfo.InvariantCulture);

            // Check if learner exists
            var learnerShareRequest = new LearnerShareRequest
            {
                MobileNumber = mobileNumber,
                DateOfBirth = dob
            };

            var response = await PostDataAsync("learner/share-request", learnerShareRequest);

            if(response is {IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK})
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var learnerShareResponse = JsonSerializer.Deserialize<LearnerShareResponse>(responseContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return View("LearnerShareRequestSent", learnerShareResponse);
            }

            return RedirectToAction("Index");
        }


        [HttpPost("GetData")]
        public async Task<IActionResult> GetDataAsync(LearnerShareResponse request)
        {
            var response = await GetDataAsync<LearnerData>($"learner-data?correlationId={request.CorrelationId}");

           var result = new LearnerShareResponse()
            {
                CorrelationId = request.CorrelationId,
                Code = request.Code,
                LearnerData = response.Data,
                HttpStatusCode = response.StatusCode
            };
        

            return View("LearnerShareRequestSent", result);
        }

        private async Task<bool> CheckIfLearnerExistsForEducationRecordAsync(string mobileNumber, DateTime dob)
        {
           var learnerShareRequest = new LearnerShareRequest
            {
                MobileNumber = mobileNumber,
                DateOfBirth = dob
            };
            var response = await PostDataAsync("learner/check", learnerShareRequest);

            if(response is {IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK})
            {
                return true;
            }

            return false;
        }
    }
}
