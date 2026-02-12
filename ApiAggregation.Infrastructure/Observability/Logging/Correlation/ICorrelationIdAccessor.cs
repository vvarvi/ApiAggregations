
namespace ApiAggregation.Infrastructure.Observability.Logging.Correlation
{
    public interface ICorrelationIdAccessor
    {
        string? CorrelationId { get; set; }
    }
}
