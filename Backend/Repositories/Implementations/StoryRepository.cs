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

        public async Task<StoryViewModel?> GetByIdAsync(int id)
        {
            var story = await _baseRepo.GetFirstOrDefaultAsync(
                s => s.Id == id,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Language
            );

            return story?.ToViewModel();
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

    }
}
