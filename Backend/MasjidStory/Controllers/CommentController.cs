using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        // GET: api/Comment/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<List<CommentViewModel>>> GetAll()
        {
            var comments = await _service.GetAllCommentsAsync();
            return Ok(comments);
        }

        // GET: api/Comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentViewModel>> GetById(int id)
        {
            var comment = await _service.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        // POST: api/Comment/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CommentCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddCommentAsync(model);
            return Ok();
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentEditViewModel model)
        {
            if (id != model.Id) return BadRequest("ID mismatch.");
            var result = await _service.UpdateCommentAsync(model);
            if (!result) return NotFound();
            return Ok();
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteCommentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
