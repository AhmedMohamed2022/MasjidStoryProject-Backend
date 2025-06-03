using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Repositories;

namespace Repositories.Implementations
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IBaseRepository<Story> _baseRepo;

        public StoryRepository(IBaseRepository<Story> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public async Task<List<StoryViewModel>> GetAllAsync()
        {
            // Eager load related data (User, Masjid, Language)
            var stories = await _baseRepo.GetAllAsync(
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Language
            );

            return stories.Select(s => s.ToViewModel()).ToList();
        }
        public async Task<StoryViewModel?> GetByIdAsync(int id, string? currentUserId = null)
        {
            var story = await _baseRepo.GetFirstOrDefaultAsync(
                s => s.Id == id,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Language,
                s => s.Likes,
                s => s.Comments,
                s => s.Comments.Select(c => c.Author)
            );

            return story?.ToViewModel(currentUserId);
        }

        public async Task<StoryEditViewModel?> GetEditByIdAsync(int id)
        {
            var story = await _baseRepo.GetByIdAsync(id);
            return story?.ToEditViewModel();
        }

        //public async Task AddAsync(StoryCreateViewModel model)
        //{
        //    var entity = model.ToEntity();
        //    await _baseRepo.AddAsync(entity);
        //    await _baseRepo.SaveChangesAsync();
        //}

        public async Task<bool> UpdateAsync(StoryEditViewModel model)
        {
            var entity = await _baseRepo.GetByIdAsync(model.Id);
            if (entity == null) return false;

            model.UpdateEntity(entity);
            _baseRepo.Update(entity);
            await _baseRepo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _baseRepo.Delete(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }
        public async Task AddStoryAsync(StoryCreateViewModel model, string userId)
        {
            var entity = model.ToEntity(userId);
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();
        }
        public async Task<List<StoryViewModel>> GetPendingAsync()
        {
            var stories = await _baseRepo.FindAsync(
                s => !s.IsApproved,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Language
            );

            return stories.Select(s => s.ToViewModel()).ToList();
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var story = await _baseRepo.GetByIdAsync(id);
            if (story == null) return false;

            story.IsApproved = true;
            _baseRepo.Update(story);
            await _baseRepo.SaveChangesAsync();
            return true;
        }
        public async Task<List<StoryViewModel>> GetLatestApprovedStoriesAsync(int count)
        {
            var stories = await _baseRepo.GetAllAsync(
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Language
            );

            return stories
                .Where(s => s.IsApproved)
                .OrderByDescending(s => s.DatePublished)
                .Take(count)
                .Select(s => s.ToViewModel())
                .ToList();
        }


    }
}
