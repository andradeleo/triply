using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(TokenService tokenService) : ControllerBase
    {
        public record LoginRequest(string Username, string Password);

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // ATENÇÃO: validação fake só pra desenvolver o fluxo.
            // Depois troque por consulta ao banco + verificação de hash de senha.
            if (request.Username != "admin" || request.Password != "123456")
                return Unauthorized();

            var token = tokenService.GenerateToken(
                userId: "1",
                username: request.Username,
                roles: ["Admin"]);

            return Ok(new { token });
        }
    }
}
