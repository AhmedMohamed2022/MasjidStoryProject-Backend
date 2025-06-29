using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;
using Repositories.Interfaces;
using System.Security.Claims;
using ViewModels;

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
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DateCreated)
                .Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Title = n.Title,
                    Message = n.Text,
                    Type = n.Type,
                    ContentType = n.ContentType,
                    ContentId = n.ContentId,
                    IsRead = n.IsRead,
                    DateCreated = n.DateCreated,
                    SenderName = n.SenderName
                })
                .ToListAsync();

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
                Text = model.Message,
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
                Message = notification.Text,
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
            var notification = new Notification
            {
                UserId = contentOwnerId,
                Title = "New Like",
                Text = $"{likerName} liked your {contentType.ToLower()}",
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
            var notification = new Notification
            {
                UserId = contentOwnerId,
                Title = "New Comment",
                Text = $"{commenterName} commented on your {contentType.ToLower()}",
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
        public async Task CreateApprovalNotificationAsync(string userId, string title, string message, string contentType, int? contentId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Text = message,
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