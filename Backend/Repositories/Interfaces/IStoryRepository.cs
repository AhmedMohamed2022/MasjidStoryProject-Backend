using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<StoryViewModel>> GetAllAsync(string languageCode = "en");
        Task<StoryViewModel?> GetByIdAsync(int id, string? currentUserId = null, string languageCode = "en");
        //Task<StoryEditViewModel?> GetEditByIdAsync(int id);
        Task<bool> UpdateAsync(StoryEditViewModel model);
        Task<bool> DeleteAsync(int id);
        //Task AddStoryAsync(StoryCreateViewModel model, string userId, List<string> imageUrls);
        Task<int> AddStoryAsync(StoryCreateViewModel model,string userId);
        Task<List<StoryViewModel>> GetPendingAsync(string languageCode = "en");
        Task<bool> ApproveAsync(int id);
        Task<List<StoryViewModel>> GetLatestApprovedStoriesAsync(int count, string languageCode = "en");
        Task<List<StoryViewModel>> GetRelatedStoriesAsync(int storyId, string languageCode = "en");
        Task<List<StoryViewModel>> GetStoriesByUserIdAsync(string userId, string languageCode = "en");
        Task<PaginatedResponse<StoryViewModel>> GetPaginatedAsync(int page, int size, string languageCode = "en");

    }

}
