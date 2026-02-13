using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Caching
{
    public class FakeApiClient : IExternalApiClient
    {
        private readonly string _name;

        public FakeApiClient(string name)
        {
            _name = name;
        }

        public string SourceName => _name;

        public Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<AggregatedItem>>(new[]
            {
            new AggregatedItem { Source = _name, Title = "Test" }
        });
        }
    }
}
