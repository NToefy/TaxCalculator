using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class PostalLookupDTO
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public string TaxCalculationType { get; set; }
        public string TaxCalculationDescriptor { get; set; }
    }
}
