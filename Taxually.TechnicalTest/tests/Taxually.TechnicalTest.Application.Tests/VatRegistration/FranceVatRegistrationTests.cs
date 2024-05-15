using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Strategies;

namespace Taxually.TechnicalTest.Application.Tests.VatRegistration
{
    [TestClass]
    public class FranceVatRegistrationTests
    {
        private IVatRegistrationStrategy? _franceRegistrationStrategy;
        private Mock<ITaxuallyQueueClient> _mockQueueClient;

        [TestInitialize]
        public void Setup()
        {
            _mockQueueClient = new Mock<ITaxuallyQueueClient>();
            var config = Options.Create(new VatRegistrationHandlingOptions() { FranceQueueName = "test-queue-name" });
            var loggerMock = new Mock<ILogger<FranceVatRegistrationStrategy>>();
            _franceRegistrationStrategy = new FranceVatRegistrationStrategy(
            _mockQueueClient.Object,
             config,
             loggerMock.Object
             );
        }

        [TestMethod]
        public async Task FranceVatRegistrationShouldSucceed()
        {
            _mockQueueClient.Setup(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<byte[]>()));

            await _franceRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "FR"
            });
            _mockQueueClient
                .Verify(x => x.EnqueueAsync("test-queue-name", It.IsAny<byte[]>()), Times.Once);
        }

        [TestMethod]
        public async Task FranceVatRegistrationShouldSucceed_IfCountryCodeIsLower()
        {
            _mockQueueClient.Setup(x => x.EnqueueAsync("test-queue-name", It.IsAny<byte[]>()));

            await _franceRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "fr"
            });
            _mockQueueClient
                .Verify(x => x.EnqueueAsync("test-queue-name", It.IsAny<byte[]>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedCountryException))]
        public async Task FranceVatRegistrationShouldThrow_IfCountryCodeIsInvalid()
        {
            await _franceRegistrationStrategy.RegisterAsync(new Application.VatRegistration.VatRegistrationRequest()
            {
                CompanyId = "Some-Company-Id",
                CompanyName = "Some-Company-Name",
                Country = "uk"
            });
        }
    }
}
