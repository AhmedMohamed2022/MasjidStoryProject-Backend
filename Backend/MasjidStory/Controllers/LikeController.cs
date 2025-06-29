using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("toggle")]
        [Authorize]
        public async Task<IActionResult> ToggleLike([FromBody] LikeCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }
            
            var liked = await _likeService.ToggleLikeAsync(model.ContentId, model.ContentType, userId);
            return Ok(new { success = true, liked });
        }

        [HttpGet("count/{contentType}/{contentId}")]
        public async Task<IActionResult> GetLikeCount(string contentType, int contentId)
        {
            var count = await _likeService.GetLikeCountAsync(contentId, contentType);
            return Ok(new { success = true, count });
        }

        [HttpGet("status/{contentType}/{contentId}")]
        [Authorize]
        public async Task<IActionResult> GetLikeStatus(string contentType, int contentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }
            
            var isLiked = await _likeService.IsLikedByUserAsync(contentId, contentType, userId);
            return Ok(new { success = true, isLiked });
        }
    }
}
