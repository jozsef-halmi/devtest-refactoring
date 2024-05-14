using Taxually.TechnicalTest.Application.VatRegistration;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Application.Interfaces
{
    public interface IVatRegistrationStrategy
    {
        Task RegisterAsync(VatRegistrationRequest request);
        Country GetSupportedCountry();
    }
}
