using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Security.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string role);
    }
}
