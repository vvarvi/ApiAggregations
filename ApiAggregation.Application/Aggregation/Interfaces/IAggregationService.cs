using ApiAggregation.Domain.Models;

namespace ApiAggregation.Application.Aggregation.Interfaces
{
    public interface IAggregationService
    {
        Task<IEnumerable<AggregatedItem>> AggregateAsync(AggregationQuery query, CancellationToken cancellationToken);
    }
}
