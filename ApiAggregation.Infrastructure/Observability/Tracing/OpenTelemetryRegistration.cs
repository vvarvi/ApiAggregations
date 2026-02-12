using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ApiAggregation.Infrastructure.Observability.Tracing
{
    public static class OpenTelemetryRegistration
    {
        public static IServiceCollection AddOpenTelemetryTracing(
        this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("ApiAggregationService"))
                    //.AddAspNetCoreInstrumentation()
                    //.AddHttpClientInstrumentation()
                    //.AddConsoleExporter();
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();
                });

            return services;
        }
    }
}
