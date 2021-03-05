using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class RateLookupDTO
    {
        public int Id { get; set; }
        public string Rate { get; set; }
        public decimal? FromLimit { get; set; }
        public decimal? ToLimit { get; set; }
        public decimal? RateCalcVal { get; set; }
    }
}
