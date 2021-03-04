using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Models;

namespace TaxCalculator.Repositories
{
    public interface IPostalRepository
    {
        Task<List<PostalLookupDTO>> GetAllPostalLookupsAsync();
    }
}
