using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class CalculationsResultDTO
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public decimal? AnnualIncome { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public decimal? CalculatedTax { get; set; }
        public string CalculationType { get; set; }
    }
}
