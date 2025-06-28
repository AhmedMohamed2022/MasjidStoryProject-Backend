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
        [HttpGet("{id}")]
        public async Task<ActionResult<EventViewModel>> GetById(int id)
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var ev = await _service.GetEventDetailsAsync(id, userId);
            if (ev == null) return NotFound();

            return Ok(ApiResponse<EventViewModel>.Ok(ev));
        }

        [HttpGet("masjid/{masjidId}")]
        public async Task<ActionResult<List<EventViewModel>>> GetMasjidEvents(int masjidId)
        {
            var result = await _service.GetMasjidEventsAsync(masjidId);
            return Ok(ApiResponse<List<EventViewModel>>.Ok(result));
        }

        [HttpGet("my-registrations")]
        [Authorize]
        public async Task<ActionResult<List<EventViewModel>>> GetMyRegistrations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = await _service.GetUserRegisteredEventsAsync(userId);
            return Ok(ApiResponse<List<EventViewModel>>.Ok(events));
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] EventCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _service.UpdateEventAsync(id, model, userId);
            if (!updated) return Forbid("Only creator can edit.");
            return Ok(ApiResponse<string>.Ok("Event updated successfully."));
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleted = await _service.DeleteEventAsync(id, userId);
            if (!deleted) return Forbid("Only creator can delete.");
            return Ok(ApiResponse<string>.Ok("Event deleted successfully."));
        }

    }

}
