using System.Collections.Generic;
using System.Linq;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Coterie.UnitTests.ControllerTests
{
    public class RateControllerShould : RateControllerTestsBase
    {
        public static object[] BadGetCases =
        {
            new object[] { null, "Failed to parse request" },
            new object[] { new GetRateRequest(), "Must provide at least one state" },
            new object[] { new GetRateRequest{States = new List<string>{"NotReal", "TX"}}, "Invalid State(s) provided: NotReal"},
            new object[] { new GetRateRequest{States = new List<string>{"TX"}}, "Revenue must be a positive whole number"},
            new object[] { new GetRateRequest{States = new List<string>{"TX"}, Revenue = 1, Business = (BusinessTypes) 55}, "55 is not a valid business type"},
            
        };
        
        [TestCaseSource(nameof(BadGetCases))]
        public void BadRequestReturns400(GetRateRequest request, string errorMessage)
        {
            var actual = rateController.CalculateRate(request);

            Assert.NotNull(actual.Result);

            var badRequestResult = actual.Result as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);

            var exception = badRequestResult.Value as BaseExceptionResponse;
            
            Assert.AreEqual(exception, badRequestResult.Value);
        }

        [Test]
        public void ValidRequestReturnsRateResponse()
        {
            rateServiceMock.Setup(x => x.GetRate(It.Is<GetRateRequest>(y => y.Business == BusinessTypes.Plumber
                                                                            && y.Revenue == 30000 && y.States.FirstOrDefault() == "Texas")))
                .Returns(new GetRateResponse
                {
                    Business = BusinessTypes.Plumber,
                    Revenue = 30000,
                    Premiums = new List<PremiumDetails>
                    {
                        new() { Premium = 123, State = States.TX }
                    }
                });
            
            var actual = rateController.CalculateRate(new GetRateRequest
            {
                Business = BusinessTypes.Plumber,
                Revenue = 30000,
                States = new List<string> { "Texas" }
            });

            Assert.NotNull(actual.Value);
            Assert.AreEqual(30000, actual.Value.Revenue);
            Assert.AreEqual(BusinessTypes.Plumber, actual.Value.Business);
            Assert.AreEqual(1, actual.Value.Premiums.Count);
        }
    }
}