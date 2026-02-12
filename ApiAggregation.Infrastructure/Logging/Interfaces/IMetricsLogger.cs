
namespace ApiAggregation.Infrastructure.Logging.Interfaces
{
    public interface IMetricsLogger
    {
        void IncrementExternalApiCall(string source);
    }
}
