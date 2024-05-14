using Taxually.TechnicalTest.Application.VatRegistration;

namespace Taxually.TechnicalTest.Application.Interfaces.VatRegistration
{
  public interface IVatRegistrationStrategyContext
    {
        Task ExecuteStrategyAsync(VatRegistrationRequest request);
    }
}
