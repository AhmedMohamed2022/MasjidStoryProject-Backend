using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _service;

        public StoryController(StoryService service)
        {
            _service = service;
        }

        // GET: api/story/all
        [HttpGet("all")]
        public async Task<ActionResult<List<StoryViewModel>>> GetAll()
        {
            var stories = await _service.GetAllStoriesAsync();
            return Ok(stories);
        }

        // GET: api/story/details/{id}
        [HttpGet("details/{id}")]
        public async Task<ActionResult<StoryViewModel>> GetById(int id)
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var story = await _service.GetStoryByIdAsync(id, userId);
            if (story == null)
                return NotFound();

            return Ok(story);
        }

        // POST: api/story/add
        [Authorize]
        [HttpPost("add")]
        [RequestSizeLimit(10 * 1024 * 1024)] // Max 10 MB total for uploaded images
        public async Task<IActionResult> AddStory([FromForm] StoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _service.AddStoryAsync(model, userId);
            return Ok(ApiResponse<string>.Ok("Story submitted and pending approval."));
        }

        // PUT: api/story/update/{id}
        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] StoryEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");

            var result = await _service.UpdateStoryAsync(model);
            if (!result)
                return NotFound();

            return Ok(ApiResponse<string>.Ok("Story updated successfully."));
        }

        // DELETE: api/story/delete/{id}
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteStoryAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/story/pending
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<StoryViewModel>>> GetPendingApproval()
        {
            var stories = await _service.GetPendingStoriesAsync();
            return Ok(stories);
        }

        // PUT: api/story/approve/{id}
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveStory(int id)
        {
            var result = await _service.ApproveStoryAsync(id);
            if (!result) return NotFound();

            return Ok(ApiResponse<string>.Ok("Story Approved."));
        }

        // GET: api/story/latest
        [HttpGet("latest")]
        public async Task<ActionResult<List<StoryViewModel>>> GetLatestStories()
        {
            var stories = await _service.GetLatestStoriesAsync();
            return Ok(ApiResponse<List<StoryViewModel>>.Ok(stories));
        }

        // GET: api/story/related/{storyId}
        [HttpGet("related/{storyId}")]
        public async Task<ActionResult<List<StoryViewModel>>> GetRelatedStories(int storyId)
        {
            var relatedStories = await _service.GetRelatedStoriesAsync(storyId);
            return Ok(ApiResponse<List<StoryViewModel>>.Ok(relatedStories));
        }
    }
}
