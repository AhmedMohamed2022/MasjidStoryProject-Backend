using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;
namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null) return BadRequest("Registration failed.");
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null) return Unauthorized("Invalid credentials.");
            return Ok(result);
        }
    }
}
