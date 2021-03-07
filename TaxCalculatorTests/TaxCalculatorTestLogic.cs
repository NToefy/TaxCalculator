using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TaxCalculator.Models;

namespace TaxCalculatorTests
{
    class TaxCalculatorTestLogic
    {
        private Mock<TaxCalculator.Repositories.TaxCalculatorRepository> taxCalculatorRepository;
        private List<RateLookupDTO> rateValues;

        [SetUp]
        public void Setup()
        {
            taxCalculatorRepository = new Mock<TaxCalculator.Repositories.TaxCalculatorRepository>();
            rateValues = new List<RateLookupDTO>();
        }

        [Test]
        public async Task TestGetAllRateLookups()
        {
            //rateValues = taxCalculatorRepository.Setup(a => a.GetAllRateLookupsAsync().Result).Returns(rateValues);
            //var mockUnit = new Mock<TaxCalculator.Repositories.ITaxCalculatorRepository>();
            //mockUnit.Setup(x => x.GetAllRateLookupsAsync()).ReturnsAsync(rateValues);
            //var taxrep = new Mock<TaxCalculator.Repositories.TaxCalculatorRepository>();
            //var test = taxrep.Setup(a => a.GetAllRateLookupsAsync().ConfigureAwait(true).GetAwaiter().GetResult());
            //Assert.IsTrue(test.Returns(rateValues) > 0);
        }
    }
}
