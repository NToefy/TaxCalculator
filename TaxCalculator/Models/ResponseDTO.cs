using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class ResponseDTO
    {
        public string status { get; set; }
        public string message { get; set; }
        public decimal taxValue { get; set; }
        public string typeOfCalculation { get; set; }
    }
}
