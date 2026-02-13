using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Caching
{
    public class FakeTtlPolicy : ICacheTtlPolicy
    {
        public TimeSpan GetTtl(string sourceName)
            => TimeSpan.FromMinutes(5);
    }
}
