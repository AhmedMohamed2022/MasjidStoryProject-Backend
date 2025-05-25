using Microsoft.AspNetCore.Mvc;
using Services;
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

        // POST: api/story/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] StoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddStoryAsync(model);
            return Ok();
        }

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
    }
}
