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
        private readonly IPostalRepository _postalRepository;
        public TaxController(IPostalRepository postalRepository)
        {
            _postalRepository = postalRepository;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllPostalLookupsAsync()
        {
            var result = await _postalRepository.GetAllPostalLookupsAsync();
            return Ok(result);
        }
    }
}
