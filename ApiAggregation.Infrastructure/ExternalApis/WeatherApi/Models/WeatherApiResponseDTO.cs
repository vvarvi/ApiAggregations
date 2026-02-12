using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.ExternalApis.WeatherApi.Models
{
    public class WeatherApiResponseDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public decimal Temperature { get; set; }
    }
}
