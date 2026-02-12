using ApiAggregation.Infrastructure.Observability.Logging.Correlation;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Infrastructure.Observability.Logging
{
    public static class LoggingRegistration
    {
        public static IServiceCollection AddCorrelationLogging(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();

            return services;
        }
    }
}
