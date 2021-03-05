using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class PostParamsDTO
    {
        public string postalCode { get; set; }
        public decimal annualIncome { get; set; }
    }
}
