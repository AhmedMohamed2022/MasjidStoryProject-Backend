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
            var story = await _service.GetStoryByIdAsync(id);
            if (story == null)
                return NotFound();

            return Ok(story);
        }

        //// POST: api/story/create
        //[HttpPost("create")]
        //public async Task<IActionResult> Create([FromBody] StoryCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    await _service.AddStoryAsync(model);
        //    return Ok();
        //}

        // PUT: api/story/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StoryEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");

            var result = await _service.UpdateStoryAsync(model);
            if (!result)
                return NotFound();

            return Ok();
        }

        // DELETE: api/story/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteStoryAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddStory([FromBody] StoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Get user ID from identity (in real app, this must be authenticated)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _service.AddStoryAsync(model, userId);
            return Ok("Story submitted and pending approval.");
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

            return Ok("Story approved successfully.");
        }


    }
}
