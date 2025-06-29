using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly CommunityService _service;

        public CommunityController(CommunityService service)
        {
            _service = service;
        }

        [HttpGet("masjid/{masjidId}")]
        public async Task<ActionResult<List<CommunityViewModel>>> GetMasjidCommunities(int masjidId)
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var list = await _service.GetMasjidCommunitiesAsync(masjidId, userId);
            return Ok(ApiResponse<List<CommunityViewModel>>.Ok(list));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityViewModel>> GetById(int id)
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var item = await _service.GetByIdAsync(id, userId);
            if (item == null) return NotFound();
            return Ok(ApiResponse<CommunityViewModel>.Ok(item));
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CommunityCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _service.CreateAsync(model, userId);
            return Ok(ApiResponse<string>.Ok("Community created."));
        }

        [HttpPost("{id}/join")]
        [Authorize]
        public async Task<IActionResult> Join(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.JoinCommunityAsync(id, userId);
            if (!success) return BadRequest("Already joined.");
            return Ok(ApiResponse<string>.Ok("Joined community."));
        }

        [HttpPost("{id}/leave")]
        [Authorize]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.LeaveCommunityAsync(id, userId);
            if (!success) return BadRequest("Not a member.");
            return Ok(ApiResponse<string>.Ok("Left community."));
        }

        [HttpGet("my-communities")]
        [Authorize]
        public async Task<ActionResult<List<CommunityViewModel>>> GetMyCommunities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetUserCommunitiesAsync(userId);
            return Ok(ApiResponse<List<CommunityViewModel>>.Ok(result));
        }
        // Update community
        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] CommunityCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _service.UpdateCommunityAsync(id, model, userId);
            if (!updated) return Forbid("Only creator can edit.");
            return Ok(ApiResponse<string>.Ok("Community updated successfully."));
        }

        // Delete community
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleted = await _service.DeleteCommunityAsync(id, userId);
            if (!deleted) return Forbid("Only creator can delete.");
            return Ok(ApiResponse<string>.Ok("Community deleted successfully."));
        }

        // Get all communities 
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<CommunityViewModel>>> GetAllCommunities()
        {
            var communities = await _service.GetAllCommunitiesAsync();
            return Ok(ApiResponse<List<CommunityViewModel>>.Ok(communities));
        }
    }

}
