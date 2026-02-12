using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Application.Aggregation
{
    public class AggregationQuery
    {
        public string? Category { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SortBy { get; set; }
    }
}
