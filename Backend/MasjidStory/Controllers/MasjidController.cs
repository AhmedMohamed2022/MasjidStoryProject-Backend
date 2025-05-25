using Microsoft.AspNetCore.Mvc;
using ViewModels;
using Services;
namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasjidController : ControllerBase
    {
        private readonly MasjidService _service;

        public MasjidController(MasjidService service)
        {
            _service = service;
        }

        [HttpGet ("getAll")]
        public async Task<ActionResult<List<MasjidViewModel>>> GetAll()
        {
            var masjids = await _service.GetAllMasjidsAsync();
            return Ok(masjids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasjidViewModel>> GetById(int id)
        {
            var masjid = await _service.GetMasjidByIdAsync(id);
            if (masjid == null) return NotFound();
            return Ok(masjid);
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<MasjidViewModel>>> Search([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetMasjidsPagedAsync(query, page, size);
            return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MasjidCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddMasjidAsync(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MasjidEditViewModel model)
        {
            if (id != model.Id) return BadRequest("ID mismatch.");
            var result = await _service.UpdateMasjidAsync(model);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteMasjidAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
