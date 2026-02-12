using ApiAggregation.Infrastructure.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Logging
{
    public class MetricsLogger: IMetricsLogger
    {
        private readonly ILogger<MetricsLogger> _logger;

        public MetricsLogger(ILogger<MetricsLogger> logger)
        {
            _logger = logger;
        }

        public void IncrementExternalApiCall(string source)
        {
            _logger.LogInformation("Metric: ExternalApiCall {Source}", source);
        }
    }
}
