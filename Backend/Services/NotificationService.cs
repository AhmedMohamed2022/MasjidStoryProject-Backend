using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;
using Repositories.Interfaces;
using System.Security.Claims;
using ViewModels;
using System.Text.Json;

namespace Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository<Notification> _notificationRepository;

        public NotificationService(ApplicationDbContext context, IBaseRepository<Notification> notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationViewModel>> GetUserNotificationsAsync(string userId)
        {
            var notificationEntities = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            var notifications = notificationEntities.Select(n => new NotificationViewModel
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                MessageKey = n.MessageKey,
                MessageVariables = n.MessageVariables != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(n.MessageVariables) : null,
                Type = n.Type,
                ContentType = n.ContentType,
                ContentId = n.ContentId,
                IsRead = n.IsRead,
                DateCreated = n.DateCreated,
                SenderName = n.SenderName
            }).ToList();

            return notifications;
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NotificationViewModel> CreateNotificationAsync(NotificationCreateViewModel model)
        {
            var notification = new Notification
            {
                UserId = model.UserId,
                Title = model.Title,
                MessageKey = model.MessageKey,
                MessageVariables = model.MessageVariables != null ? JsonSerializer.Serialize(model.MessageVariables) : null,
                Type = model.Type,
                ContentType = model.ContentType,
                ContentId = model.ContentId,
                IsRead = false,
                DateCreated = DateTime.UtcNow,
                SenderName = model.SenderName
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return new NotificationViewModel
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                MessageKey = notification.MessageKey,
                MessageVariables = notification.MessageVariables != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(notification.MessageVariables) : null,
                Type = notification.Type,
                ContentType = notification.ContentType,
                ContentId = notification.ContentId,
                IsRead = notification.IsRead,
                DateCreated = notification.DateCreated,
                SenderName = notification.SenderName
            };
        }

        // Helper method to create notifications for likes
        public async Task CreateLikeNotificationAsync(int contentId, string contentType, string contentOwnerId, string likerName)
        {
            var variables = new Dictionary<string, string>
            {
                { "user", likerName },
                { "contentType", contentType.ToLower() }
            };
            var notification = new Notification
            {
                UserId = contentOwnerId,
                Title = "New Like",
                MessageKey = "NOTIFICATION.LIKE",
                MessageVariables = JsonSerializer.Serialize(variables),
                Type = "Like",
                ContentType = contentType,
                ContentId = contentId,
                IsRead = false,
                DateCreated = DateTime.UtcNow,
                SenderName = likerName
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // Helper method to create notifications for comments
        public async Task CreateCommentNotificationAsync(int contentId, string contentType, string contentOwnerId, string commenterName)
        {
            var variables = new Dictionary<string, string>
            {
                { "user", commenterName },
                { "contentType", contentType.ToLower() }
            };
            var notification = new Notification
            {
                UserId = contentOwnerId,
                Title = "New Comment",
                MessageKey = "NOTIFICATION.COMMENT",
                MessageVariables = JsonSerializer.Serialize(variables),
                Type = "Comment",
                ContentType = contentType,
                ContentId = contentId,
                IsRead = false,
                DateCreated = DateTime.UtcNow,
                SenderName = commenterName
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // Helper method to create approval notifications
        public async Task CreateApprovalNotificationAsync(string userId, string title, string messageKey, Dictionary<string, string> variables, string contentType, int? contentId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                MessageKey = messageKey,
                MessageVariables = variables != null ? JsonSerializer.Serialize(variables) : null,
                Type = "Approval",
                ContentType = contentType,
                ContentId = contentId,
                IsRead = false,
                DateCreated = DateTime.UtcNow,
                SenderName = "System"
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
} 