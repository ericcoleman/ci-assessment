using System;
using System.Collections.Generic;
using System.Linq;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Shared;
using NUnit.Framework;

namespace Coterie.UnitTests
{
    public class RateServiceShould : RateServiceTestsBase
    {
        [Test]
        public void CanCalculateRates()
        {
            //Arrange
            var rateRequest = new GetRateRequest
            {
                Business = BusinessTypes.Plumber,
                Revenue = 6000000,
                States = new List<string> { "tx", "OH", "Florida" }
            };
            
            // Act
            var actual = RateService.GetRate(rateRequest);
            
            // Assert
            Assert.AreEqual(BusinessTypes.Plumber, actual.Business);
            Assert.True(actual.IsSuccessful);
            Assert.AreEqual(6000000, actual.Revenue);

            Assert.AreEqual(3, actual.Premiums.Count);
            
            Assert.AreEqual(11316, actual.Premiums.First(x => x.State == States.TX).Premium);
            Assert.AreEqual(12000, actual.Premiums.First(x => x.State == States.OH).Premium);
            Assert.AreEqual(14400, actual.Premiums.First(x => x.State == States.FL).Premium);
            
            Assert.IsNotNull(actual);
        }
    }
}