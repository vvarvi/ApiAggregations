using ApiAggregation.Infrastructure.Caching;
using ApiAggregation.Infrastructure.Configurations;
using ApiAggregation.Infrastructure.Logging;
using ApiAggregation.Infrastructure.Logging.Interfaces;
using ApiAggregation.Infrastructure.Observability.Logging;
using ApiAggregation.Infrastructure.Observability.Tracing;
using ApiAggregation.Infrastructure.Performance;
using ApiAggregation.Infrastructure.Security.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheTTLOptions>(configuration.GetSection("CacheTtlPolicy"));

            services.AddMemoryCache();

            services.AddScoped<ICacheTtlPolicy, ConfigurableCacheTtlPolicy>();

            services.AddHttpClients();

            services.AddExternalApiClients();

            services.AddSingleton<IMetricsLogger, MetricsLogger>();

            services.AddOpenTelemetryTracing();

            services.AddCorrelationLogging();

            services.AddJwtSecurity(configuration);

            services.AddSingleton<IApiPerformanceTracker, InMemoryApiPerformanceTracker>();

            services.AddSingleton<PerformanceAnalyzerHostedService>();

            services.AddHostedService<PerformanceLoggingHostedService>();

            return services;
        }
    }
}
