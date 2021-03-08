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

        public async Task<List<PostalLookupDTO>> GetAllPostalLookupsAsync()
        {
            
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string query = "SELECT * FROM PostalLookup";
                    List<PostalLookupDTO> postalLookups = (await conn.QueryAsync<PostalLookupDTO>(sql: query)).ToList();
                    return postalLookups;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<RateLookupDTO>> GetAllRateLookupsAsync()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    //string query = "SELECT * FROM RateLookup1"; // Simulate fail
                    string query = "SELECT * FROM RateLookup";
                    List<RateLookupDTO> rateLookups = (await conn.QueryAsync<RateLookupDTO>(sql: query)).ToList();
                    return rateLookups;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<TaxModelDTO> GetAllRateAndPostalLookupsAsync()
        {
            try
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
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<PostalLookupDTO> GetTaxRateDescriptorByPostalCodeAsync(string postalCode)
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

        public async Task<ResponseDTO> CalculateTaxAsync(string postalCode, decimal annualIncome)
        {
            try
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



                switch (postalCodeDescriptor)
                {
                    case "FR":
                        decimal calPercFR = 0.175M;
                        taxToPay = annualIncome * calPercFR;



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

                        if (result == 1)
                        {
                            response.status = "success";
                            response.message = "Tax calculation performed successfully.";
                            response.taxValue = taxToPay;
                            response.typeOfCalculation = "Flat Rate";
                            response.totalAfterTax = annualIncome - taxToPay;
                            response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                        }
                        else
                        {
                            response.status = "error";
                            response.message = "Error saving result to the database.";
                            response.taxValue = taxToPay;
                            response.typeOfCalculation = "Flat Rate";
                            response.totalAfterTax = annualIncome - taxToPay;
                            response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                        }


                        break;
                    case "FV":
                        decimal calPercFV = 0.05M;

                        if (annualIncome > 0 && annualIncome < 200000)
                        {
                            taxToPay = annualIncome * calPercFV;
                        }
                        else
                        {
                            taxToPay = 10000;
                        }




                        // Write to the database

                        CalculationsResultDTO calcResultFV = new CalculationsResultDTO
                        {
                            PostalCode = postalCode,
                            AnnualIncome = annualIncome,
                            DateSubmitted = DateTime.Now,
                            CalculatedTax = taxToPay,
                            CalculationType = "Flat Value"
                        };

                        var resultFV = await SaveTaxResultAsync(calcResultFV);

                        if (resultFV == 1)
                        {
                            response.status = "success";
                            response.message = "Tax calculation performed successfully.";
                            response.taxValue = taxToPay;
                            response.typeOfCalculation = "Flat Value";
                            response.totalAfterTax = annualIncome - taxToPay;
                            response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                        }
                        else
                        {
                            response.status = "error";
                            response.message = "Error saving result to the database.";
                            response.taxValue = taxToPay;
                            response.typeOfCalculation = "Flat Value";
                            response.totalAfterTax = annualIncome - taxToPay;
                            response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                        }

                        break;
                    case "P":
                        var rateTableData = await GetAllRateLookupsAsync();
                        if (rateTableData != null)
                        {
                            decimal tempTaxCalculated = 0;

                            foreach (var bracket in rateTableData)
                            {
                                decimal? fromValue = bracket.FromLimit;
                                decimal? toValue = bracket.ToLimit;
                                decimal? calcRate = bracket.RateCalcVal;

                                tempTaxCalculated += CalculateTaxPerBracket(annualIncome, fromValue, toValue, calcRate);

                            }

                            taxToPay = tempTaxCalculated;

                            // Write to the database

                            CalculationsResultDTO calcResultP = new CalculationsResultDTO
                            {
                                PostalCode = postalCode,
                                AnnualIncome = annualIncome,
                                DateSubmitted = DateTime.Now,
                                CalculatedTax = taxToPay,
                                CalculationType = "Progressive"
                            };

                            var resultP = await SaveTaxResultAsync(calcResultP);

                            if (resultP == 1)
                            {
                                response.status = "success";
                                response.message = "Tax calculation performed successfully.";
                                response.taxValue = taxToPay;
                                response.typeOfCalculation = "Progressive";
                                response.totalAfterTax = annualIncome - taxToPay;
                                response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                            }
                            else
                            {
                                response.status = "error";
                                response.message = "Error saving result to the database.";
                                response.taxValue = taxToPay;
                                response.typeOfCalculation = "Progressive";
                                response.totalAfterTax = annualIncome - taxToPay;
                                response.totalTaxPercentage = (taxToPay / annualIncome) * 100;
                            }

                        }
                        else
                        {
                            response.status = "Incomplete";
                            response.message = "Rate Table not populated. Tax calculation cannot be performed.";
                            return response;
                        }

                        break;
                    default:
                        break;
                }


                return response;
            }
            catch (Exception ex)
            {

                ResponseDTO responseError = new ResponseDTO();
                responseError.status = "Error";
                responseError.message = ex.Message;
                responseError.taxValue = 0;
                responseError.typeOfCalculation = "None";
                responseError.totalAfterTax = 0;
                responseError.totalTaxPercentage = 0;

                return responseError;
            }
            
        }

        public async Task<int> SaveTaxResultAsync(CalculationsResultDTO calcResult)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
            
        }

        public decimal CalculateTaxPerBracket(decimal annualIncome, decimal? fromValue, decimal? toValue, decimal? rateCalValue)
        {
            try
            {
                decimal taxToPayCalculated = 0;
                decimal? bracketAmount = 0;
                decimal? bracketTaxAmount = 0;

                if (annualIncome > toValue)
                {
                    // Calculate Tax For Full Bracket
                    bracketAmount = toValue - (fromValue == 0 ? 0 : (fromValue - 1));
                    bracketTaxAmount = bracketAmount * rateCalValue;

                    taxToPayCalculated = (decimal)(taxToPayCalculated + bracketTaxAmount);
                    return taxToPayCalculated;
                }

                if (annualIncome >= fromValue && annualIncome <= toValue)
                {
                    bracketAmount = annualIncome - (fromValue == 0 ? 0 : (fromValue - 1));
                    bracketTaxAmount = bracketAmount * rateCalValue;

                    taxToPayCalculated = (decimal)(taxToPayCalculated + bracketTaxAmount);
                    return taxToPayCalculated;
                }

                if (toValue == null && annualIncome >= fromValue)
                {
                    bracketAmount = annualIncome - (fromValue == 0 ? 0 : (fromValue - 1));
                    bracketTaxAmount = bracketAmount * rateCalValue;

                    taxToPayCalculated = (decimal)(taxToPayCalculated + bracketTaxAmount);
                    return taxToPayCalculated;
                }


                return taxToPayCalculated;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
