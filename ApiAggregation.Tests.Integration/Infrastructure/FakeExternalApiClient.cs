using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Tests.Integration.Infrastructure
{
    public class FakeExternalApiClient : IExternalApiClient
    {
        public string SourceName => "FakeAPI";

        public Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken ct)
        {
            return Task.FromResult<IEnumerable<AggregatedItem>>(new[]
            {
            new AggregatedItem
            {
                Title = "Fake Item",
                Source = SourceName
            }
        });
        }
    }
}
