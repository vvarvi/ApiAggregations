using ApiAggregation.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace ApiAggregation.Infrastructure.Caching
{
    public class ConfigurableCacheTtlPolicy : ICacheTtlPolicy
    {
        private readonly CacheTTLOptions _options;

        public ConfigurableCacheTtlPolicy(IOptions<CacheTTLOptions> options)
        {
            _options = options.Value;
        }

        public TimeSpan GetTtl(string sourceName)
        {
            if (_options.Sources.TryGetValue(sourceName, out var minutes))
            {
                return TimeSpan.FromMinutes(minutes);
            }

            return TimeSpan.FromMinutes(_options.DefaultTTLMinutes);
        }
    }
}
