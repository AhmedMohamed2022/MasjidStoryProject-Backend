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
        private readonly MediaService _mediaService;

        public StoryService(IStoryRepository repository, MediaService mediaService)
        {
            _repository = repository;
            _mediaService = mediaService;
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

        public async Task<List<StoryViewModel>> GetAllStoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StoryViewModel?> GetStoryByIdAsync(int id, string? userId = null)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task AddStoryAsync(StoryCreateViewModel model)
        {
            await _repository.AddStoryAsync(model);
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
            return await _repository.ApproveAsync(id);
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


    }
}
