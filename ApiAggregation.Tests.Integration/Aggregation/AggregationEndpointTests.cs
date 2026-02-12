using ApiAggregation.Domain.Models;
using ApiAggregation.Tests.Integration.Infrastructure;
using FluentAssertions;
using System.Net.Http.Json;

namespace ApiAggregation.Tests.Integration.Aggregation
{
    public class AggregationEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AggregationEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GET_aggregation_returns_aggregated_items()
        {
            var response = await _client.GetAsync("/api/aggregation");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<AggregatedItem>>();

            result.Should().NotBeNull();
            result.Should().Contain(x => x.Title == "Fake Item");
        }
    }
}
