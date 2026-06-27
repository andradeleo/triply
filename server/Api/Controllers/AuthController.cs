using Application.UseCases.Authentication;
using Communication.Request;
using Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAccessTokenGenerator tokenService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin request)
        {
            var useCase = new LoginUseCase(tokenService);

            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
