using Application.UseCases.Authentication;
using Communication.Request;
using Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseLogin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
