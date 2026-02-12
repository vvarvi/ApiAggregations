using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Observability.Logging.Correlation
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-ID";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICorrelationIdAccessor accessor, ILogger<CorrelationIdMiddleware> logger)
        {
            var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            accessor.CorrelationId = correlationId;

            context.Response.Headers[HeaderName] = correlationId;

            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                await _next(context);
            }
        }
    }
}
