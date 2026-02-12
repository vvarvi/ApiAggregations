namespace ApiAggregation.Infrastructure.Configurations
{
    public class CacheTTLOptions
    {
        public int DefaultTTLMinutes { get; set; } = 5;
        public Dictionary<string, int> Sources { get; set; } = new();
    }
}
