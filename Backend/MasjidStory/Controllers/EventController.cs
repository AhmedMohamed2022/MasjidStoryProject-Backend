using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _service;

        public EventController(EventService service)
        {
            _service = service;
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<EventViewModel>>> GetUpcomingEvents()
        {
            var events = await _service.GetUpcomingEventsAsync();
            return Ok(ApiResponse<List<EventViewModel>>.Ok(events));
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _service.AddEventAsync(model, userId);
            return Ok(ApiResponse<string>.Ok("Event created"));
        }

        [HttpPost("{id}/register")]
        [Authorize]
        public async Task<IActionResult> RegisterToEvent(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await _service.RegisterUserToEventAsync(id, userId);
            if (!success) return BadRequest("You are already registered.");

            return Ok(ApiResponse<string>.Ok("Registered successfully."));
        }
    }

}
