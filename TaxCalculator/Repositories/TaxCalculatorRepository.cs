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

        public async Task<PostalLookupDTO> GetTaxRateDescriptorByPostalCodeAsync(string postalCode)
        {
            using (IDbConnection conn = Connection)
            {
                string query = "SELECT * FROM PostalLookup WHERE PostalCode = @postalCode";
                PostalLookupDTO postalLookup = await conn.QueryFirstOrDefaultAsync<PostalLookupDTO>(sql: query, param: new { postalCode });
                return postalLookup;
            }
        }

        public async Task<ResponseDTO> CalculateTaxAsync(string postalCode, decimal annualIncome)
        {
            ResponseDTO response = new ResponseDTO();
            decimal taxToPay = 0;

            var postalCodeRecord = await GetTaxRateDescriptorByPostalCodeAsync(postalCode);
            string postalCodeDescriptor = postalCodeRecord == null ? string.Empty : postalCodeRecord.TaxCalculationDescriptor;

            if (string.IsNullOrEmpty(postalCodeDescriptor))
            {
                response.status = "Incomplete";
                response.message = "Postal code does not exist in reference data.";
                return response;
            }

            var rateTableData = await GetAllRateLookupsAsync();

            if (rateTableData != null)
            {
                switch (postalCodeDescriptor)
                {
                    case "FR":
                        decimal calPercFR = 0.175M;
                        taxToPay = annualIncome * calPercFR;

                        response.status = "success";
                        response.message = "Tax calculation performed successfully.";
                        response.taxValue = taxToPay;
                        response.typeOfCalculation = "Flat Rate";

                        // Write to the database

                        CalculationsResultDTO calcResult = new CalculationsResultDTO
                        {
                            PostalCode = postalCode,
                            AnnualIncome = annualIncome,
                            DateSubmitted = DateTime.Now,
                            CalculatedTax = taxToPay,
                            CalculationType = "Flat Rate"
                        };

                        var result = await SaveTaxResultAsync(calcResult);
                        


                        break;
                    case "FV":
                        // code block
                        break;
                    case "P":
                        // code block
                        break;
                    default:
                        break;
                }
            }
            else
            {
                response.status = "Incomplete";
                response.message = "Rate Table not populated. Tax calculation cannot be performed.";
                return response;
            }

            return response;
        }

        public async Task<int> SaveTaxResultAsync(CalculationsResultDTO calcResult)
        {
            using (IDbConnection conn = Connection)
            {
                string command = @"
				INSERT INTO TaxCalculationsResult([PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType])
				VALUES(@PostalCode, @AnnualIncome, @DateSubmitted, @CalculatedTax, @CalculationType)";

                var result = await conn.ExecuteAsync(sql: command, param: calcResult);
                return result;
            }
        }
    }
}
