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
        Task<List<StoryViewModel>> GetAllAsync();
        Task<StoryViewModel?> GetByIdAsync(int id, string? currentUserId = null);
        Task<StoryEditViewModel?> GetEditByIdAsync(int id);
        //Task AddAsync(StoryCreateViewModel model);
        Task<bool> UpdateAsync(StoryEditViewModel model);
        Task<bool> DeleteAsync(int id);
        Task AddStoryAsync(StoryCreateViewModel model, string userId);
        Task<List<StoryViewModel>> GetPendingAsync();
        Task<bool> ApproveAsync(int id);
        Task<List<StoryViewModel>> GetLatestApprovedStoriesAsync(int count);

    }
}
