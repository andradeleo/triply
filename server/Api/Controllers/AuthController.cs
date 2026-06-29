using Application.UseCases.Authentication;
using Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
