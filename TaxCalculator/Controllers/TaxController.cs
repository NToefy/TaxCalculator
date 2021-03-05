using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Repositories;
using System.Text.Json;

namespace TaxCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxCalculatorRepository _taxCalculatorRepository;
        public TaxController(ITaxCalculatorRepository taxCalculatorRepository)
        {
            _taxCalculatorRepository = taxCalculatorRepository;
        }

        [HttpGet]
        [Route("get-all-postal-lookups")]
        public async Task<IActionResult> GetAllPostalLookupsAsync()
        {
            var result = await _taxCalculatorRepository.GetAllPostalLookupsAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("get-all-rate-lookups")]
        public async Task<IActionResult> GetAllRateLookupsAsync()
        {
            var result = await _taxCalculatorRepository.GetAllRateLookupsAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("get-all-lookups")]
        public async Task<IActionResult> GetAllRateAndPostalLookupsAsync()
        {
            var result = await _taxCalculatorRepository.GetAllRateAndPostalLookupsAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("get-rate-lookup-item-by-id")]
        public async Task<IActionResult> GetTaxRateDescriptorByPostalCodeAsync([FromBody] JToken requestData)
        {
            var postalCode = requestData["input"].ToObject<string>();
            //var postalCode = requestData;

            var result = await _taxCalculatorRepository.GetTaxRateDescriptorByPostalCodeAsync(postalCode);
            return Ok(result);
        }

    }
}
