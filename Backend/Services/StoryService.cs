using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class StoryService
    {
        private readonly IStoryRepository _repository;

        public StoryService(IStoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StoryViewModel>> GetAllStoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StoryViewModel?> GetStoryByIdAsync(int id, string? userId = null)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        //public async Task AddStoryAsync(StoryCreateViewModel model)
        //{
        //    await _repository.AddAsync(model);
        //}

        public async Task<bool> UpdateStoryAsync(StoryEditViewModel model)
        {
            return await _repository.UpdateAsync(model);
        }

        public async Task<bool> DeleteStoryAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task AddStoryAsync(StoryCreateViewModel model, string userId)
        {
            await _repository.AddStoryAsync(model, userId);
        }
        public async Task<List<StoryViewModel>> GetPendingStoriesAsync()
        {
            return await _repository.GetPendingAsync();
        }

        public async Task<bool> ApproveStoryAsync(int id)
        {
            return await _repository.ApproveAsync(id);
        }
        public async Task<List<StoryViewModel>> GetLatestStoriesAsync()
        {
            var stories = await _repository.GetLatestApprovedStoriesAsync(6);
            return stories;
        }

    }
}
