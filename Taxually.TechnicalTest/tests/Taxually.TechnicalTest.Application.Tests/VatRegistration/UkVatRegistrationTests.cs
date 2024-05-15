using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Strategies;

namespace Taxually.TechnicalTest.Application.Tests.VatRegistration
{
    [TestClass]
    public class UKVatRegistrationTests
    {
        private IVatRegistrationStrategy? _ukRegistrationStrategy;
        private Mock<ITaxuallyHttpClient> _mockHttpClient;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpClient = new Mock<ITaxuallyHttpClient>();
            var config = Options.Create(new VatRegistrationHandlingOptions() { UkApiBaseUrl = "http://localhost/" });
            var loggerMock = new Mock<ILogger<UKVatRegistrationStrategy>>();
           
            _ukRegistrationStrategy = new UKVatRegistrationStrategy(
             _mockHttpClient.Object,
             config,
             loggerMock.Object
             );
        }

        [TestMethod]
        public async Task UKVatRegistrationShouldSucceed()
        {
            _mockHttpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<VatRegistrationRequest>()));

            await _ukRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "UK"
            });

            _mockHttpClient
               .Verify(x => x.PostAsync("http://localhost/", It.IsAny<VatRegistrationRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task UKVatRegistrationShouldSucceed_IfCountryCodeIsLower()
        {
            _mockHttpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<VatRegistrationRequest>()));

            await _ukRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "uk"
            });

            _mockHttpClient
               .Verify(x => x.PostAsync("http://localhost/", It.IsAny<VatRegistrationRequest>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedCountryException))]
        public async Task UKVatRegistrationShouldThrow_IfCountryCodeIsInvalid()
        {
            await _ukRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "de"
            });
        }
    }
}
