using ApiAggregation.Application.Aggregation.Interfaces;
using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using Microsoft.Extensions.Logging;

namespace ApiAggregation.Application.Aggregation
{
    public class AggregationService : IAggregationService
    {
        private readonly IEnumerable<IExternalApiClient> _externalApiClients;

        private readonly ILogger<AggregationService> _logger;

        public AggregationService(IEnumerable<IExternalApiClient> externalApiClients, ILogger<AggregationService> logger)
        {
            _externalApiClients = externalApiClients;
            _logger = logger;
        }

        public async Task<IEnumerable<AggregatedItem>> AggregateAsync(AggregationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Aggregation started");

                var tasks = _externalApiClients.Select(client =>
                    FetchSafeAsync(client, cancellationToken));

                var results = await Task.WhenAll(tasks);

                var combined = results.SelectMany(r => r);

                var filtered = ApplyFiltering(combined, query);
                var sorted = ApplySorting(filtered, query);

                _logger.LogInformation("Aggregation service completed");

                return sorted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during aggregation service...");
                throw;
            }
        }

        private async Task<IEnumerable<AggregatedItem>> FetchSafeAsync(IExternalApiClient client, CancellationToken cancellationToken)
        {
            try
            {
                return await client.FetchAsync(cancellationToken);
            }
            catch
            {
                // Fail gracefully → return empty for that API
                return Enumerable.Empty<AggregatedItem>();
            }
        }

        private IEnumerable<AggregatedItem> ApplyFiltering(IEnumerable<AggregatedItem> items, AggregationQuery query)
        {
            if (!string.IsNullOrEmpty(query.Category))
                items = items.Where(x => x.Category == query.Category);

            if (query.FromDate.HasValue)
                items = items.Where(x => x.Date >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                items = items.Where(x => x.Date <= query.ToDate.Value);

            return items;
        }

        private IEnumerable<AggregatedItem> ApplySorting(IEnumerable<AggregatedItem> items, AggregationQuery query)
        {
            return query.SortBy?.ToLower() switch
            {
                "date" => items.OrderByDescending(x => x.Date),
                "title" => items.OrderBy(x => x.Title),
                "source" => items.OrderBy(x => x.Source),
                _ => items
            };
        }
    }
}
