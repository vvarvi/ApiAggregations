using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi;
using ApiAggregation.Infrastructure.ExternalApis.WeatherApi;
using ApiAggregation.Infrastructure.Resilience;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Infrastructure.DependencyInjection
{
    public static class HttpClientRegistration
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<WeatherApiClient>()
                .AddStandardResilienceHandler(ResiliencePipelineFactory.Configure);

            services.AddHttpClient<NewsApiClient>()
                .AddStandardResilienceHandler(ResiliencePipelineFactory.Configure);

            services.AddHttpClient<GitHubApiClient>()
                .AddStandardResilienceHandler(ResiliencePipelineFactory.Configure);

            return services;
        }
    }
}
