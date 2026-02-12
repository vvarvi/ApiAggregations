
namespace ApiAggregation.Infrastructure.Caching
{
    public interface ICacheTtlPolicy
    {
        TimeSpan GetTtl(string sourceName);
    }
}
