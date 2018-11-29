using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenEx.InterviewProject.Models;

namespace TokenEx.InterviewProject.Controllers
{
    public class HomeController : Controller
    {
        private string TokenExID = "7639326364821993";
        private string ApiKey = "QRRMSOUU6q7od62bqcoW";
        private string Login = "TokenEx";
        private string Password = "TokenEx";

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // POST: Home
        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            //if the user supplied valid data, perform a credit card authorization
            if (ModelState.IsValid || !String.IsNullOrEmpty(model.Token))
            {
                try
                {
                    //implement call to payment services API and return auth code to user
                    string authCode = "ABC123";

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var requestData = new
                    {
                        APIKey = this.ApiKey,
                        TokenExID = this.TokenExID,
                        TransactionType = 1,
                        TransactionRequest = new
                        {
                            gateway = new
                            {
                                name = "LitleGateway",
                                merchant_id = this.TokenExID,
                                login = this.Login,
                                password = this.Password
                            },
                            credit_card = new
                            {
                                number = model.Token,
                                month = model.ExpMonth,
                                year = model.ExpYear,
                                verification_value = model.CVV,
                                first_name = model.FirstName,
                                last_name = model.LastName
                            },
                            transaction = new
                            {
                                amount = 1000
                            }
                        }
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("https://test-api.tokenex.com/PaymentServices.svc/REST/ProcessTransaction"),
                        Content = content,
                        Method = HttpMethod.Post
                    };

                    var response = client.SendAsync(request);

                    var result = response.GetAwaiter().GetResult().Content.ReadAsAsync<JObject>().GetAwaiter().GetResult();

                    if (result != null)
                    {
                        if (result.TryGetValue("ReferenceNumber", out var number))
                        {
                            authCode = number.ToString();
                        }
                    }

                    ViewBag.AuthCode = authCode;
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult GetAuthenticationKey()
        {
            var timeStamp = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
            var hmac = new System.Security.Cryptography.HMACSHA256();

            hmac.Key = System.Text.Encoding.UTF8.GetBytes("QRRMSOUU6q7od62bqcoW");

            var concatenatedInfo = "7639326364821993|" + "http://localhost"
                 + $"|{timeStamp}|sixANTOKENfour";
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(concatenatedInfo));
            var authKey = Convert.ToBase64String(hash);

            var result = new 
            {
                TimeStamp = timeStamp,
                AuthenticationKey = authKey
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}