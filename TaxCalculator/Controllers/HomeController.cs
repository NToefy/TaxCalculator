using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Models;
using System.Web;
using RestSharp;
using Nancy.Json;
using Newtonsoft.Json;

namespace TaxCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string firstName, string lastName)
        {
            ViewBag.Name = string.Format("Name: {0} {1}", firstName, lastName);
            return View();
        }

        [HttpPost]
        public IActionResult CalculateTax(string postalCode, decimal annualIncome)
        {
            // Get values from form

            // Validate values

            // Include Exception handling

            // Set initial value

            // Get postalLookup data based on postal code. pass through postal code to lookup. Not get all

            // Get full lookup from rates lookup. Calculation to be based on all rows of rates lookup

            decimal taxValue = 0;

            string url = "http://localhost:49991/api/Tax";
            var input = new ParamTestDTO { postalCode = postalCode };
            //var input = postalCode;

            

            var restClient = new RestClient()
            {
                BaseUrl = new Uri(url),
                Timeout = 45000
            };


            var jsonbody = new JavaScriptSerializer().Serialize(input);

            var restRequest = new RestRequest();

            restRequest.Method = Method.POST;
            restRequest.AddHeader("Cache-Control", "no-cache");
            restRequest.AddHeader("Content-Type", @"application/json");
            restRequest.Resource = "get-rate-lookup-item-by-id";
            restRequest.AddJsonBody(input);

            var response = restClient.Execute<string>(restRequest);

            //var baseUrl = "http://localhost:49991/api/Tax";
            //var client = new RestClient(baseUrl);
            //var request = new RestRequest("/get-rate-lookup-item-by-id", Method.POST);
            //request.AddHeader("Content-Type", @"application/json");
            //request.RequestFormat = DataFormat.Json;
            //request.AddParameter("Application/Json", input, ParameterType.RequestBody);

            //var response = client.Execute(request);



            //string taxDescriptor = _taxController.ControllerContext.

            ViewBag.Values = string.Format("Values: {0} {1}", postalCode, annualIncome);
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
