using Repositories.Interfaces;
using ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    /// <summary>
    /// Service layer for business logic related to Comments.
    /// </summary>
    public class CommentService
    {
        private readonly ICommentRepository _repo;
        private readonly NotificationService _notificationService;
        private readonly ApplicationDbContext _context;

        public CommentService(ICommentRepository repo, NotificationService notificationService, ApplicationDbContext context)
        {
            _repo = repo;
            _notificationService = notificationService;
            _context = context;
        }

        public async Task<CommentViewModel> AddCommentAsync(CommentCreateViewModel model, string userId)
        {
            var comment = await _repo.AddAsync(model, userId);
            
            // Create notification for the comment
            await CreateCommentNotificationAsync(model.ContentId, model.ContentType, userId);
            
            return comment;
        }

        /// <summary>
        /// Get comments by content type and ID (Story, Event, Community)
        /// </summary>
        public async Task<List<CommentViewModel>> GetCommentsByContentAsync(int contentId, string contentType)
        {
            return await _repo.GetByContentAsync(contentId, contentType);
        }

        /// <summary>
        /// Get comments by story ID (for backward compatibility)
        /// </summary>
        public async Task<List<CommentViewModel>> GetCommentsByStoryAsync(int storyId)
        {
            return await _repo.GetByContentAsync(storyId, "Story");
        }

        public async Task<List<CommentViewModel>> GetAllCommentsAsync() => await _repo.GetAllAsync();
        public async Task<CommentViewModel?> GetCommentByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<bool> UpdateCommentAsync(CommentEditViewModel model) => await _repo.UpdateAsync(model);
        public async Task<bool> DeleteCommentAsync(int id) => await _repo.DeleteAsync(id);

        /// <summary>
        /// Create notification for new comment
        /// </summary>
        private async Task CreateCommentNotificationAsync(int contentId, string contentType, string commenterId)
        {
            try
            {
                // Get content owner ID and commenter name
                string contentOwnerId = await GetContentOwnerIdAsync(contentId, contentType);
                string commenterName = await GetUserNameAsync(commenterId);

                // Don't create notification if user comments on their own content
                if (contentOwnerId == commenterId) return;

                await _notificationService.CreateCommentNotificationAsync(contentId, contentType, contentOwnerId, commenterName);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the comment operation
                Console.WriteLine($"Error creating comment notification: {ex.Message}");
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
    }
}
