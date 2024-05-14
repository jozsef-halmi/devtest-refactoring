using Microsoft.Extensions.Configuration;
using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration.Strategies;
using Taxually.TechnicalTest.Strategies;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVatRegistrationStrategyContext, VatRegistrationStrategyContext>();
            services.AddTransient<IVatRegistrationStrategy, FranceVatRegistrationStrategy>();
            services.AddTransient<IVatRegistrationStrategy, GermanyVatRegistrationStrategy>();
            services.AddTransient<IVatRegistrationStrategy, UKVatRegistrationStrategy>();
            return services;
        }
    }
}
