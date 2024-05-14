using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Application.VatRegistration.Strategies
{
    public class VatRegistrationStrategyContext : IVatRegistrationStrategyContext
    {
        private readonly IEnumerable<IVatRegistrationStrategy> _strategies;

        public VatRegistrationStrategyContext(IEnumerable<IVatRegistrationStrategy> strategies)
        {
            _strategies = strategies;
        }

        public Task ExecuteStrategyAsync(
            VatRegistrationRequest request)
        {
            var instance = _strategies.FirstOrDefault(x =>
            x.GetSupportedCountry().Equals(Country.From(request.Country.ToUpperInvariant())));

            return instance != null ? instance.RegisterAsync(request) : throw new UnsupportedCountryException(request.Country);
        }
    }
}
