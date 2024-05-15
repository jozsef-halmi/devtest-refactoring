using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Strategies
{
    public class GermanyVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly string _queueName;
        private readonly ILogger _logger;

        public GermanyVatRegistrationStrategy(ITaxuallyQueueClient queueClient, IOptions<VatRegistrationHandlingOptions> options, ILogger<GermanyVatRegistrationStrategy> logger)
        {
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            _queueName = options.Value.GermanyQueueName ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            try
            {
                // TODO: Refactor validation to validator classes
                if (request.Country.ToUpper() != GetSupportedCountry().Code.ToUpper())
                {
                    throw new UnsupportedCountryException(request.Country);
                }

                using var stringwriter = new StringWriter();
                var xmlSerializer = new XmlSerializer(typeof(VatRegistrationRequest));
                xmlSerializer.Serialize(stringwriter, request);
                var xml = stringwriter.ToString();
                // Queue xml doc to be processed
                await _queueClient.EnqueueAsync(_queueName, xml);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured during VAT registration: {message}. Country: {country}", ex.Message, request.Country);
                throw;
            }
        }

        public Country GetSupportedCountry()
        {
            return Country.Germany;
        }
    }
}
