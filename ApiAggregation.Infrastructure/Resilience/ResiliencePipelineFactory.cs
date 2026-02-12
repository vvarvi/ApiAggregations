using Microsoft.Extensions.Http.Resilience;

namespace ApiAggregation.Infrastructure.Resilience
{
    public static class ResiliencePipelineFactory
    {
        public static void Configure(HttpStandardResilienceOptions options)
        {
            options.Retry.MaxRetryAttempts = 3;
            options.Retry.Delay = TimeSpan.FromMilliseconds(200);

            options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);

            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(15);
            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;
        }
    }
}
