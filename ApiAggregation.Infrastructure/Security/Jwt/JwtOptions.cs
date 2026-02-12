using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Security.Jwt
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
    }
}
