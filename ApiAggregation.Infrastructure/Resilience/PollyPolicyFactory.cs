using Polly;
using Polly.Extensions.Http;

namespace ApiAggregation.Infrastructure.Resilience
{
    public static class PollyPolicyFactory
    {
        public static IAsyncPolicy<HttpResponseMessage> Create()
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromMilliseconds(200 * retryAttempt));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(3);

            var circuitBreaker = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(15));

            return Policy.WrapAsync(retryPolicy, timeoutPolicy, circuitBreaker);
        }
    }
}
