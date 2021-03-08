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
using Newtonsoft.Json.Linq;

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
        //public IActionResult CalculateTax([FromBody] PostParamsDTO requestData)
        {

            try
            {
                string url = "http://localhost:49991/api/Tax";

                var input = new PostParamsDTO
                {
                    postalCode = postalCode,
                    annualIncome = annualIncome
                };


                var restClient = new RestClient()
                {
                    BaseUrl = new Uri(url),
                    Timeout = 45000
                };

                var restRequest = new RestRequest();

                restRequest.Method = Method.POST;
                restRequest.AddHeader("Cache-Control", "no-cache");
                restRequest.AddHeader("Content-Type", @"application/json");
                restRequest.Resource = "CalcTaxBasedOnIncome";
                restRequest.AddJsonBody(input);

                var response = restClient.Execute<string>(restRequest);

                ResponseDTO responseVal = JsonConvert.DeserializeObject<ResponseDTO>(response.Content);

                ViewBag.PostalCode = string.Format("Postal Code : {0}", string.IsNullOrEmpty(postalCode) == true ? "None" : postalCode);
                ViewBag.AnnualIncome = string.Format("Annual Income : {0}", annualIncome);
                ViewBag.Status = string.Format("Status : {0}", responseVal.status);
                ViewBag.Message = string.Format("Message : {0}", responseVal.message);
                ViewBag.TaxValue = string.Format("Tax Value : {0:0.##}", responseVal.taxValue);
                ViewBag.CalcType = string.Format("Type Of Calculation : {0}", responseVal.typeOfCalculation ?? "None");
                ViewBag.TotalAfterTax = string.Format("Total After Tax : {0:0.##}", responseVal.totalAfterTax);
                ViewBag.TotalTaxPercentage = string.Format("Total Tax Percentage : {0:0.##} %", responseVal.totalTaxPercentage);
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Status = string.Format("Status : {0}", "Incomplete");
                ViewBag.Message = string.Format("Message : {0}", ex.Message);
                return View("Index");
                //return Json(new { Success = false, Message = ex.Message });
            }

            
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
