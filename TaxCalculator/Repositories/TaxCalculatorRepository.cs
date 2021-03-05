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

        public async Task<List<RateLookupDTO>> GetAllRateLookupsAsync()
        {
            using (IDbConnection conn = Connection)
            {
                string query = "SELECT * FROM RateLookup";
                List<RateLookupDTO> rateLookups = (await conn.QueryAsync<RateLookupDTO>(sql: query)).ToList();
                return rateLookups;
            }
        }

        public async Task<TaxModelDTO> GetAllRateAndPostalLookupsAsync()
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"
				SELECT * FROM PostalLookup
                SELECT * FROM RateLookup";

                var reader = await conn.QueryMultipleAsync(sql: query);

                return new TaxModelDTO
                {
                    postalLookups = (await reader.ReadAsync<PostalLookupDTO>()).ToList(),
                    rateLookups = (await reader.ReadAsync<RateLookupDTO>()).ToList()
                };
            }
        }

        public async Task<RateLookupDTO> GetTaxRateDescriptorByPostalCodeAsync(string postalCode)
        {
            using (IDbConnection conn = Connection)
            {
                string query = "SELECT * FROM PostalLookup WHERE PostalCode = @postalCode";
                RateLookupDTO rateLookup = await conn.QueryFirstOrDefaultAsync<RateLookupDTO>(sql: query, param: new { postalCode });
                return rateLookup;
            }
        }
    }
}
