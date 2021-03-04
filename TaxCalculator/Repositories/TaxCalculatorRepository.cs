using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using TaxCalculator.Models;

namespace TaxCalculator.Repositories
{
    public class TaxCalculatorRepository : ITaxCalculatorRepository
    {
        private string DBContextConnection = string.Empty;


        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(DBContextConnection);
            }
        }

        public TaxCalculatorRepository(IConfiguration configuration)
        {
            DBContextConnection = configuration.GetConnectionString("DBContextConnection");
        }

        public async Task<List<PostalLookupDTO>> GetAllPostalLookupsAsync()
        {
            using (IDbConnection conn = Connection)
            {
                string query = "SELECT * FROM PostalLookup";
                List<PostalLookupDTO> postalLookups = (await conn.QueryAsync<PostalLookupDTO>(sql: query)).ToList();
                return postalLookups;
            }
        }
    }
}
