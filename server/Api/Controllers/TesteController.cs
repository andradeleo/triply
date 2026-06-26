using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet("publico")]
        public IActionResult Publico() => Ok("Qualquer um acessa.");

        [Authorize]
        [HttpGet("seguro")]
        public IActionResult Seguro() => Ok($"Olá, {User.Identity?.Name}!");

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult SoAdmin() => Ok("Área restrita a Admin.");
    }
}
