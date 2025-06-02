using Microsoft.AspNetCore.Mvc;
using ViewModels;
using Services;
using Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasjidViewModel>> GetById(int id)
        {
            var masjid = await _service.GetMasjidByIdAsync(id);
            if (masjid == null) return NotFound();
            return Ok(ApiResponse<MasjidViewModel>.Ok(masjid));
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<MasjidViewModel>>> Search([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetMasjidsPagedAsync(query, page, size);
            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(result));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MasjidCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddMasjidAsync(model);
            return Ok("Masjid Created Sucessfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MasjidEditViewModel model)
        {
            if (id != model.Id) return BadRequest("ID mismatch.");
            var result = await _service.UpdateMasjidAsync(model);
            if (!result) return NotFound();
            return Ok("Masjid Updated Sucessfully");
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteMasjidAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet]
        [Route("{id}/details")]
        public async Task<ActionResult<MasjidDetailsViewModel>> GetMasjidDetails(int id, [FromQuery] string? lang = null)
        {
            var result = await _service.GetMasjidDetailsAsync(id, lang);
            if (result == null) return NotFound();
            return Ok(ApiResponse<MasjidDetailsViewModel>.Ok(result));
        }
        [HttpPost("{id}/visit")]
        [Authorize]
        public async Task<IActionResult> RegisterVisit(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var success = await _service.RegisterVisitAsync(id, userId);
            if (!success) return NotFound(ApiResponse<string>.Fail("Masjid not found."));

            return Ok(ApiResponse<string>.Ok("Visit recorded."));
        }


    }

}
