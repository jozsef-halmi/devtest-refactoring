using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Strategies
{
    public class UKVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyHttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ILogger _logger;

        public UKVatRegistrationStrategy(ITaxuallyHttpClient httpClient, IOptions<VatRegistrationHandlingOptions> options, ILogger<UKVatRegistrationStrategy> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = options.Value.UkApiBaseUrl ?? throw new ArgumentNullException(nameof(options));
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

                // TODO: Taxually.TechnicalTest request model should not
                // depend on the model of the external service -> 
                // Introduce another model and the corresponding mapping
                await _httpClient.PostAsync(_apiBaseUrl, request);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured during VAT registration: {message}. Country: {country}", ex.Message, request.Country);
                throw;
            }
        }

        public Country GetSupportedCountry()
        {
            return Country.UnitedKingdom;
        }
    }
}
