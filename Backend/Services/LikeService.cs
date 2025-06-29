using Repositories.Interfaces;
using ViewModels;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    /// <summary>
    /// Service layer for handling business logic related to Likes
    /// </summary>
    public class LikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly NotificationService _notificationService;
        private readonly ApplicationDbContext _context;

        public LikeService(ILikeRepository likeRepository, NotificationService notificationService, ApplicationDbContext context)
        {
            _likeRepository = likeRepository;
            _notificationService = notificationService;
            _context = context;
        }

        /// <summary>
        /// Toggle like for any content type (Story, Event, Community)
        /// </summary>
        public async Task<bool> ToggleLikeAsync(int contentId, string contentType, string userId)
        {
            var result = await _likeRepository.ToggleLikeAsync(contentId, contentType, userId);
            
            // If a like was added (not removed), create notification
            if (result)
            {
                await CreateLikeNotificationAsync(contentId, contentType, userId);
            }
            
            return result;
        }

        /// <summary>
        /// Get like count for any content type
        /// </summary>
        public async Task<int> GetLikeCountAsync(int contentId, string contentType)
        {
            return await _likeRepository.GetLikeCountAsync(contentId, contentType);
        }

        /// <summary>
        /// Check if user has liked the content
        /// </summary>
        public async Task<bool> IsLikedByUserAsync(int contentId, string contentType, string userId)
        {
            return await _likeRepository.IsLikedByUserAsync(contentId, contentType, userId);
        }

        /// <summary>
        /// Create notification for new like
        /// </summary>
        private async Task CreateLikeNotificationAsync(int contentId, string contentType, string likerId)
        {
            try
            {
                // Get content owner ID and liker name
                string contentOwnerId = await GetContentOwnerIdAsync(contentId, contentType);
                string likerName = await GetUserNameAsync(likerId);

                // Don't create notification if user likes their own content
                if (contentOwnerId == likerId) return;

                await _notificationService.CreateLikeNotificationAsync(contentId, contentType, contentOwnerId, likerName);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the like operation
                Console.WriteLine($"Error creating like notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Get content owner ID based on content type
        /// </summary>
        private async Task<string> GetContentOwnerIdAsync(int contentId, string contentType)
        {
            return contentType.ToLower() switch
            {
                "story" => await _context.Stories
                    .Where(s => s.Id == contentId)
                    .Select(s => s.ApplicationUserId)
                    .FirstOrDefaultAsync() ?? "",
                    
                "event" => await _context.Events
                    .Where(e => e.Id == contentId)
                    .Select(e => e.CreatedById)
                    .FirstOrDefaultAsync() ?? "",
                    
                "community" => await _context.Communities
                    .Where(c => c.Id == contentId)
                    .Select(c => c.CreatedById)
                    .FirstOrDefaultAsync() ?? "",
                    
                _ => ""
            };
        }

        /// <summary>
        /// Get user name by ID
        /// </summary>
        private async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefaultAsync();

            return user != null ? $"{user.FirstName} {user.LastName}" : "Unknown User";
        }

        ///// <summary>
        ///// Deletes a like by ID
        ///// </summary>
        //public async Task<bool> DeleteLikeAsync(int id)
        //{
        //    return await _likeRepository.DeleteAsync(id);
        //}
    }
} 