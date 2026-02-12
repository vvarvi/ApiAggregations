using Microsoft.AspNetCore.Builder;
using Serilog;

namespace ApiAggregation.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, config) =>
            {
                config
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
