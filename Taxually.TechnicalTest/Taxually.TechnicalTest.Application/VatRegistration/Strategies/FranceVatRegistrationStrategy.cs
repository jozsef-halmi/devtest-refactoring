using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Strategies
{
    public class FranceVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly string _queueName;
        private readonly ILogger _logger;

        public FranceVatRegistrationStrategy(ITaxuallyQueueClient queueClient, IOptions<VatRegistrationHandlingOptions> options, ILogger<FranceVatRegistrationStrategy> logger)
        {
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            _queueName = options.Value.FranceQueueName ?? throw new ArgumentNullException(nameof(options));
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

                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("CompanyName,CompanyId");
                csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
                var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                // Queue file to be processed
                await _queueClient.EnqueueAsync(_queueName, csv);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured during VAT registration: {message}. Country: {country}", ex.Message, request.Country);
                throw;
            }
        }

        public Country GetSupportedCountry()
        {
            return Country.France;
        }
    }
}
