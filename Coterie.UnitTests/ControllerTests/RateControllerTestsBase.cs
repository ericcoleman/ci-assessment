using Coterie.Api.Controllers;
using Coterie.Api.Interfaces;
using Coterie.Api.Services;
using Moq;
using NUnit.Framework;

namespace Coterie.UnitTests.ControllerTests
{
    public class RateControllerTestsBase
    {
        protected RateController rateController;
        protected Mock<IRateService> rateServiceMock;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            //MockTestService = new Mock<ITestService>();
            rateServiceMock = new Mock<IRateService>();
            rateController = new RateController(rateServiceMock.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            // Sample reset after each test is ran
            //MockTestService.Reset();
        }
    }
}