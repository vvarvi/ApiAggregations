using ApiAggregation.Api.DTOs;
using ApiAggregation.Application.Aggregation;
using ApiAggregation.Application.Aggregation.Interfaces;
using ApiAggregation.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregation.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AggregationController : Controller
    {
        private readonly IAggregationService _aggregationService;

        private readonly IConfiguration _config;

        private readonly ILogger<AggregationController> _logger;

        public AggregationController(IAggregationService aggregationService, IConfiguration config, ILogger<AggregationController> logger)
        {
            _aggregationService = aggregationService;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAggregated(
        [FromQuery] string? category,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] string? sortBy,
        CancellationToken cancellationToken)
        {
            try
            {
                var query = new AggregationQuery
                {
                    Category = category,
                    FromDate = fromDate,
                    ToDate = toDate,
                    SortBy = sortBy
                };

                var results = await _aggregationService.AggregateAsync(query, cancellationToken);

                var response = results.Select(MapToDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while aggregating data.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private AggregatedItemResponseDTO MapToDto(AggregatedItem item)
        {
            return new AggregatedItemResponseDTO
            {
                Source = item.Source,
                Title = item.Title,
                Category = item.Category,
                Date = item.Date.ToString("yyyy-MM-dd"),
                Url = item.Url
            };
        }
    }
}
