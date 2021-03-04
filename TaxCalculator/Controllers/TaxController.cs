using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Repositories;

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
        [Route("get-all")]
        public async Task<IActionResult> GetAllPostalLookupsAsync()
        {
            var result = await _taxCalculatorRepository.GetAllPostalLookupsAsync();
            return Ok(result);
        }


    }
}
