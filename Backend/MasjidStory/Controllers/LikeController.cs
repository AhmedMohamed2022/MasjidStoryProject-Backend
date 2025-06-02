using Microsoft.AspNetCore.Mvc;
using Services;
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

        /// <summary>
        /// Gets all likes
        /// </summary>
        [HttpGet("getAll")]
        public async Task<ActionResult<List<LikeViewModel>>> GetAll()
        {
            var likes = await _likeService.GetAllLikesAsync();
            return Ok(likes);
        }

        /// <summary>
        /// Gets a like by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LikeViewModel>> GetById(int id)
        {
            var like = await _likeService.GetLikeByIdAsync(id);
            if (like == null) return NotFound();
            return Ok(like);
        }

        /// <summary>
        /// Creates a new like
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LikeCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _likeService.AddLikeAsync(model);
            return Ok();
        }

        /// <summary>
        /// Deletes a like by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _likeService.DeleteLikeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
