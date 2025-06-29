using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories.Implementations
{
    /// <summary>
    /// Repository to handle all Comment-related database operations.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly IBaseRepository<Comment> _baseRepo;

        public CommentRepository(IBaseRepository<Comment> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        /// <summary>
        /// Returns all comments with related Story and Author details.
        /// </summary>
        public async Task<List<CommentViewModel>> GetAllAsync()
        {
            var comments = await _baseRepo.GetAllAsync(c => c.Story, c => c.Author);
            return comments.Select(c => c.ToViewModel()).ToList();
        }

        /// <summary>
        /// Returns one comment by its ID with related navigation data.
        /// </summary>
        public async Task<CommentViewModel?> GetByIdAsync(int id)
        {
            var entity = await _baseRepo.GetFirstOrDefaultAsync(c => c.Id == id, c => c.Story, c => c.Author);
            return entity?.ToViewModel();
        }

        /// <summary>
        /// Gets comment for editing by ID
        /// </summary>
        public async Task<CommentEditViewModel?> GetEditByIdAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            return entity == null
                ? null
                : new CommentEditViewModel
                {
                    Id = entity.Id,
                    Content = entity.Content
                };
        }

        /// <summary>
        /// Adds a new comment to the database.
        /// </summary>
        public async Task<CommentViewModel> AddAsync(CommentCreateViewModel model, string userId)
        {
            var entity = model.ToEntity(userId);
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();

            // Fetch the inserted comment with navigation properties
            var inserted = await _baseRepo.GetFirstOrDefaultAsync(
                c => c.Id == entity.Id,
                c => c.Story,
                c => c.Author
            );

            // Map to ViewModel and return
            return inserted.ToViewModel();
        }


        /// <summary>
        /// Updates the content of an existing comment.
        /// </summary>
        public async Task<bool> UpdateAsync(CommentEditViewModel model)
        {
            var entity = await _baseRepo.GetByIdAsync(model.Id);
            if (entity == null) return false;

            model.UpdateEntity(entity);
            _baseRepo.Update(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a comment by ID.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _baseRepo.Delete(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }
        public async Task<List<CommentViewModel>> GetByStoryIdAsync(int storyId)
        {
            var comments = await _baseRepo.FindAsync(
                c => c.StoryId == storyId && c.IsActive,
                c => c.Author
            );

            return comments.Select(c => c.ToViewModel()).ToList();
        }

        /// <summary>
        /// Gets comments by content ID and type (for stories, events, communities)
        /// </summary>
        public async Task<List<CommentViewModel>> GetByContentAsync(int contentId, string contentType)
        {
            var comments = await _baseRepo.FindAsync(
                c => c.ContentId == contentId && c.ContentType == contentType && c.IsActive,
                c => c.Author
            );

            return comments.Select(c => c.ToViewModel()).ToList();
        }

    }
}
