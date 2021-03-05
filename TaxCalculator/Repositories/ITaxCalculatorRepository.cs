using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Models;

namespace TaxCalculator.Repositories
{
    public interface ITaxCalculatorRepository
    {
        Task<List<PostalLookupDTO>> GetAllPostalLookupsAsync();
        Task<List<RateLookupDTO>> GetAllRateLookupsAsync();
        Task<TaxModelDTO> GetAllRateAndPostalLookupsAsync();
        Task<PostalLookupDTO> GetTaxRateDescriptorByPostalCodeAsync(string postalCode);
        Task<ResponseDTO> CalculateTaxAsync(string postalCode, decimal annualIncome);
    }
}
