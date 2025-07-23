using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories.Implementations
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IBaseRepository<Story> _baseRepo;
        private readonly IBaseRepository<Media> _mediaRepo;
        private readonly IBaseRepository<Tag> _tagRepo;
        private readonly IBaseRepository<StoryTag> _storyTagRepo;
        private readonly ApplicationDbContext _context;

        public StoryRepository(
            IBaseRepository<Story> baseRepo,
            IBaseRepository<Media> mediaRepo,
            IBaseRepository<Tag> tagRepo,
            IBaseRepository<StoryTag> storyTagRepo,
            ApplicationDbContext context)
        {
            _baseRepo = baseRepo;
            _mediaRepo = mediaRepo;
            _tagRepo = tagRepo;
            _storyTagRepo = storyTagRepo;
            _context = context;
        }

        public async Task<int> AddStoryAsync(StoryCreateViewModel model, string userId)
        {
            var entity = model.ToEntity(userId);
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<StoryViewModel?> GetByIdAsync(int id, string? currentUserId = null, string languageCode = "en")
        {
            var story = await _baseRepo.GetFirstOrDefaultAsync(
                s => s.Id == id,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.Likes,
                s => s.Comments,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return story?.ToViewModel(currentUserId, languageCode);
        }

        public async Task<List<StoryViewModel>> GetAllAsync(string languageCode = "en")
        {
            var stories = await _baseRepo.GetAllAsync(
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return stories.Where(s => s.IsApproved == true).Select(s => s.ToViewModel(null, languageCode)).ToList();
        }

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
            // Load story with all dependent collections
            var story = await _baseRepo.GetFirstOrDefaultAsync(
                s => s.Id == id,
                s => s.Comments,
                s => s.Likes,
                s => s.StoryTags,
                s => s.MediaItems
            );

            if (story == null)
                return false;

            // Delete comments
            if (story.Comments != null)
            {
                foreach (var comment in story.Comments.ToList())
                    _context.Remove(comment);
            }

            // Delete likes
            if (story.Likes != null)
            {
                foreach (var like in story.Likes.ToList())
                    _context.Remove(like);
            }

            // Delete story tags
            if (story.StoryTags != null)
            {
                foreach (var tag in story.StoryTags.ToList())
                    _context.Remove(tag);
            }

            // Media items will be automatically deleted by cascade delete
            // No need to manually delete them

            // Finally delete the story itself
            _baseRepo.Delete(story);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<StoryViewModel>> GetPendingAsync(string languageCode = "en")
        {
            var stories = await _baseRepo.FindAsync(
                s => !s.IsApproved,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return stories.Select(s => s.ToViewModel(null, languageCode)).ToList();
        }

        public async Task<List<StoryViewModel>> GetLatestApprovedStoriesAsync(int count, string languageCode = "en")
        {
            var stories = await _baseRepo.FindAsync(
                s => s.IsApproved,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return stories.OrderByDescending(s => s.DatePublished)
                .Take(count)
                .Select(s => s.ToViewModel(null, languageCode))
                .ToList();
        }

        public async Task<List<StoryViewModel>> GetRelatedStoriesAsync(int storyId, string languageCode = "en")
        {
            var refStory = await _baseRepo.GetFirstOrDefaultAsync(
                s => s.Id == storyId,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.Contents
            );
            if (refStory == null) return new();
            var related = await _baseRepo.FindAsync(
                s => s.Id != storyId && s.IsApproved && s.MasjidId == refStory.MasjidId,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return related.OrderByDescending(s => s.DatePublished)
                .Take(3)
                .Select(s => s.ToViewModel(null, languageCode))
                .ToList();
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var story = await _baseRepo.GetByIdAsync<int>(id); // Only need the entity, not the viewmodel
            if (story == null) return false;
            story.IsApproved = true;
            _baseRepo.Update(story);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<StoryViewModel>> GetStoriesByUserIdAsync(string userId, string languageCode = "en")
        {
            var stories = await _baseRepo.FindAsync(
                s => s.ApplicationUserId == userId,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            return stories.Select(s => s.ToViewModel(null, languageCode)).ToList();
        }

        public async Task<PaginatedResponse<StoryViewModel>> GetPaginatedAsync(int page, int size, string languageCode = "en")
        {
            var totalCount = await _baseRepo.CountAsync(s => s.IsApproved);
            var stories = await _baseRepo.GetListWithIncludePagedAsync(
                s => s.IsApproved,
                page,
                size,
                s => s.ApplicationUser,
                s => s.Masjid,
                s => s.Masjid.Contents,
                s => s.MediaItems,
                s => s.StoryTags,
                s => s.Contents
            );
            var storyViewModels = stories.Select(s => s.ToViewModel(null, languageCode)).ToList();
            return new PaginatedResponse<StoryViewModel>(storyViewModels, totalCount, page, size);
        }
    }
}