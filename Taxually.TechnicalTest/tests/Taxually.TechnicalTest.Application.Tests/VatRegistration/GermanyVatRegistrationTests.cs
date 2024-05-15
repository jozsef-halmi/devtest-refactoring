using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Strategies;

namespace Taxually.TechnicalTest.Application.Tests.VatRegistration
{
    [TestClass]
    public class GermanyVatRegistrationTests
    {
        private IVatRegistrationStrategy? _germanyRegistrationStrategy;
        private Mock<ITaxuallyQueueClient> _mockQueueClient;

        [TestInitialize]
        public void Setup()
        {
            _mockQueueClient = new Mock<ITaxuallyQueueClient>();
            var config = Options.Create(new VatRegistrationHandlingOptions() { GermanyQueueName = "test-queue-name" });
            var loggerMock = new Mock<ILogger<GermanyVatRegistrationStrategy>>();
           
            _germanyRegistrationStrategy = new GermanyVatRegistrationStrategy(
             _mockQueueClient.Object,
             config,
             loggerMock.Object
             );
        }

        [TestMethod]
        public async Task GermanyVatRegistrationShouldSucceed()
        {
            _mockQueueClient.Setup(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<string>()));

            await _germanyRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "DE"
            });

            _mockQueueClient
               .Verify(x => x.EnqueueAsync("test-queue-name", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GermanyVatRegistrationShouldSucceed_IfCountryCodeIsLower()
        {
            _mockQueueClient.Setup(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<string>()));

            await _germanyRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "de"
            });

            _mockQueueClient
               .Verify(x => x.EnqueueAsync("test-queue-name", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedCountryException))]
        public async Task GermanyVatRegistrationShouldThrow_IfCountryCodeIsInvalid()
        {
            await _germanyRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "uk"
            });
        }
    }
}
