using Microsoft.Extensions.Configuration;
using Taxually.TechnicalTest.Application.Interfaces;
using Taxually.TechnicalTest.Infrastructure.Http;
using Taxually.TechnicalTest.Infrastructure.Messaging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
            services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();
            return services;
        }
    }
}
