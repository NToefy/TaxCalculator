using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class TaxModelDTO
    {
        public List<RateLookupDTO> rateLookups { get; set; }
        public List<PostalLookupDTO> postalLookups { get; set; }
    }
}
