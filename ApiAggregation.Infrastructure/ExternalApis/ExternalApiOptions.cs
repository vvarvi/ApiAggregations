using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.ExternalApis
{
    public class ExternalApiOptions
    {
        public ApiConfig Weather { get; set; } = new();
        public ApiConfig News { get; set; } = new();
        public ApiConfig GitHub { get; set; } = new();
    }

    public class ApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
