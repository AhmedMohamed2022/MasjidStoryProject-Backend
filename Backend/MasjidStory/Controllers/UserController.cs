using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserProfileViewModel>> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _service.GetProfileAsync(userId);
            if (profile == null) return NotFound();
            return Ok(ApiResponse<UserProfileViewModel>.Ok(profile));
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileUpdateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.UpdateProfileAsync(userId, model);
            if (!success) return NotFound();
            return Ok(ApiResponse<string>.Ok("Profile updated successfully."));
        }
    }
}
