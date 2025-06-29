using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Models.Entities;

namespace Services
{
    public class StoryService
    {
        private readonly IStoryRepository _repository;
        private readonly MediaService _mediaService;
        private readonly NotificationService _notificationService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StoryService(IStoryRepository repository, MediaService mediaService, NotificationService notificationService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mediaService = mediaService;
            _notificationService = notificationService;
            _context = context;
            _userManager = userManager;
        }

        //public async Task AddStoryAsync(StoryCreateViewModel model, string userId)
        //{
        //    var imageUrls = new List<string>();

        //    if (model.StoryImages != null)
        //    {
        //        foreach (var image in model.StoryImages)
        //        {
        //            var url = await _mediaService.UploadToPathAsync(image, "story");
        //            if (url != null)
        //                imageUrls.Add(url);
        //        }
        //    }

        //    await _repository.AddStoryAsync(model, userId, imageUrls);
        //}
        public async Task<int> AddStoryAsync(StoryCreateViewModel model,string userId)
        {
            var storyId = await _repository.AddStoryAsync(model,userId);
            
            // Create notification for admins about new pending story
            await CreateNewStoryNotificationAsync(storyId, model.Title, userId);
            
            return storyId;
        }

        public async Task<List<StoryViewModel>> GetAllStoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StoryViewModel?> GetStoryByIdAsync(int id, string? userId = null)
        {
            return await _repository.GetByIdAsync(id, userId);
        }


        public async Task<bool> UpdateStoryAsync(StoryEditViewModel model)
        {
            return await _repository.UpdateAsync(model);
        }

        public async Task<bool> DeleteStoryAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        
        public async Task<List<StoryViewModel>> GetPendingStoriesAsync()
        {
            return await _repository.GetPendingAsync();
        }

        public async Task<bool> ApproveStoryAsync(int id)
        {
            var result = await _repository.ApproveAsync(id);
            
            if (result)
            {
                // Create notification for story owner about approval
                await CreateStoryApprovalNotificationAsync(id);
            }
            
            return result;
        }
        public async Task<List<StoryViewModel>> GetLatestStoriesAsync()
        {
            var stories = await _repository.GetLatestApprovedStoriesAsync(6);
            return stories;
        }
        public async Task<List<StoryViewModel>> GetRelatedStoriesAsync(int storyId)
        {
            return await _repository.GetRelatedStoriesAsync(storyId);
        }
        public async Task<List<StoryViewModel>> GetStoriesByUserIdAsync(string userId)
        {
            return await _repository.GetStoriesByUserIdAsync(userId);
        }

        public async Task<PaginatedResponse<StoryViewModel>> GetStoriesPaginatedAsync(int page, int size)
        {
            return await _repository.GetPaginatedAsync(page, size);
        }

        // Helper methods for notifications
        private async Task CreateNewStoryNotificationAsync(int storyId, string storyTitle, string userId)
        {
            try
            {
                // Get all admin users
                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

                // Get the story author name
                var author = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.UserName)
                    .FirstOrDefaultAsync();

                foreach (var adminUser in adminUsers)
                {
                    await _notificationService.CreateNotificationAsync(new NotificationCreateViewModel
                    {
                        UserId = adminUser.Id,
                        Title = "New Story Pending Approval",
                        Message = $"A new story '{storyTitle}' by {author} is waiting for your approval.",
                        Type = "Approval",
                        ContentType = "Story",
                        ContentId = storyId,
                        SenderName = author ?? "Unknown User"
                    });
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the story creation
                Console.WriteLine($"Error creating new story notification: {ex.Message}");
            }
        }

        private async Task CreateStoryApprovalNotificationAsync(int storyId)
        {
            try
            {
                // Get the story details
                var story = await _context.Stories
                    .FirstOrDefaultAsync(s => s.Id == storyId);

                if (story != null && !string.IsNullOrEmpty(story.ApplicationUserId))
                {
                    await _notificationService.CreateNotificationAsync(new NotificationCreateViewModel
                    {
                        UserId = story.ApplicationUserId,
                        Title = "Story Approved",
                        Message = $"Your story '{story.Title}' has been approved and is now live!",
                        Type = "Approval",
                        ContentType = "Story",
                        ContentId = storyId,
                        SenderName = "Admin"
                    });
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the approval
                Console.WriteLine($"Error creating story approval notification: {ex.Message}");
            }
        }
    }
}
