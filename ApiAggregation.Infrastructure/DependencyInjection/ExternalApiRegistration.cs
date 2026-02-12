using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi;
using ApiAggregation.Infrastructure.ExternalApis.WeatherApi;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Infrastructure.DependencyInjection
{
    public static class ExternalApiRegistration
    {
        public static IServiceCollection AddExternalApiClients(this IServiceCollection services)
        {
            services.AddScoped<IExternalApiClient, WeatherApiClient>();
            services.AddScoped<IExternalApiClient, NewsApiClient>();
            services.AddScoped<IExternalApiClient, GitHubApiClient>();

            return services;
        }
    }
}
