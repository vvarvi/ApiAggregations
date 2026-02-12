using ApiAggregation.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregation.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IJwtTokenService _jwt;

        public AuthController(IJwtTokenService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login(string userId = "demo", string role = "User")
        {
            var token = _jwt.GenerateToken(userId, role);
            return Ok(new { token });
        }
    }
}
