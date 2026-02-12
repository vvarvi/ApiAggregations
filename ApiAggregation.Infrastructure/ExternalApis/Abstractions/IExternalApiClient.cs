using ApiAggregation.Domain.Models;

namespace ApiAggregation.Infrastructure.ExternalApis.Abstractions
{
    public interface IExternalApiClient
    {
        string SourceName { get; }

        Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken);
    }
}
