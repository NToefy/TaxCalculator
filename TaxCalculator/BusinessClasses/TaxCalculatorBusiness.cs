using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Models;

namespace TaxCalculator.BusinessClasses
{
    public class TaxCalculatorBusiness
    {
        private string DBContextConnection = string.Empty;


        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(DBContextConnection);
            }
        }

        public TaxCalculatorBusiness(IConfiguration configuration)
        {
            try
            {
                //DBContextConnection = configuration.GetConnectionString("DBContextConnectio"); //Simulate incorrect connection
                DBContextConnection = configuration.GetConnectionString("DBContextConnection");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PostalLookupDTO> FetchTaxRateDescriptor(string postalCode)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string query = "SELECT * FROM PostalLookup WHERE PostalCode = @postalCode";
                    PostalLookupDTO postalLookup = await conn.QueryFirstOrDefaultAsync<PostalLookupDTO>(sql: query, param: new { postalCode });
                    return postalLookup;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
