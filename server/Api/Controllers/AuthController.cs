using Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAccessTokenGenerator tokenService) : ControllerBase
    {
        public record LoginRequest(string Username, string Password);

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "123456")
                return Unauthorized();

            var user = new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Name = request.Username,
                Role = "Admin"
            };

            var token = tokenService.Generate(user);

            return Ok(new { token });
        }
    }
}
