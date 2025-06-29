using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;

        public NotificationController(NotificationService service)
        {
            _service = service;
        }

        [HttpGet("my-notifications")]
        [Authorize]
        public async Task<ActionResult<List<NotificationViewModel>>> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var notifications = await _service.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var count = await _service.GetUnreadCountAsync(userId);
            return Ok(new { success = true, count });
        }

        [HttpPost("mark-read/{id}")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var result = await _service.MarkAsReadAsync(id, userId);
            if (!result) return NotFound();
            
            return Ok(new { success = true, message = "Notification marked as read" });
        }

        [HttpPost("mark-all-read")]
        [Authorize]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            await _service.MarkAllAsReadAsync(userId);
            return Ok(new { success = true, message = "All notifications marked as read" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var result = await _service.DeleteNotificationAsync(id, userId);
            if (!result) return NotFound();
            
            return Ok(new { success = true, message = "Notification deleted" });
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var notification = await _service.CreateNotificationAsync(model);
            return Ok(new { success = true, data = notification, message = "Notification created successfully" });
        }
    }
} 