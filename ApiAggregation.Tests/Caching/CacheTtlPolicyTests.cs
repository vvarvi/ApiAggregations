using ApiAggregation.Infrastructure.Caching;
using ApiAggregation.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace ApiAggregation.Tests.Caching
{
    public class CacheTtlPolicyTests
    {
        [Fact]
        public void TtlConfigured()
        {
            var options = Options.Create(new CacheTTLOptions
            {
                DefaultTTLMinutes = 5,
                Sources = new()
                {
                    ["WeatherAPI"] = 2
                }
            });

            var policy = new ConfigurableCacheTtlPolicy(options);

            var ttl = policy.GetTtl("WeatherAPI");

            Assert.Equal(TimeSpan.FromMinutes(2), ttl);
        }

        [Fact]
        public void TtlNotConfigured()
        {
            var options = Options.Create(new CacheTTLOptions
            {
                DefaultTTLMinutes = 5
            });

            var policy = new ConfigurableCacheTtlPolicy(options);

            var ttl = policy.GetTtl("UnknownAPI");

            Assert.Equal(TimeSpan.FromMinutes(5), ttl);
        }
    }
}