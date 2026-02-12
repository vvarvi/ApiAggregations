
namespace ApiAggregation.Infrastructure.Observability.Logging.Correlation
{
    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        public string? CorrelationId { get; set; }
    }
}
