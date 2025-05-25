using Repositories.Interfaces;
using ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Service layer for business logic related to Comments.
    /// </summary>
    public class CommentService
    {
        private readonly ICommentRepository _commentRepo;

        public CommentService(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        // Get all comments (used typically by admin or moderation panel)
        public async Task<List<CommentViewModel>> GetAllCommentsAsync()
        {
            return await _commentRepo.GetAllAsync();
        }

        // Get a single comment by ID
        public async Task<CommentViewModel?> GetCommentByIdAsync(int id)
        {
            return await _commentRepo.GetByIdAsync(id);
        }

        // Add a new comment
        public async Task AddCommentAsync(CommentCreateViewModel model)
        {
            await _commentRepo.AddAsync(model);
        }

        // Update an existing comment
        public async Task<bool> UpdateCommentAsync(CommentEditViewModel model)
        {
            return await _commentRepo.UpdateAsync(model);
        }

        // Delete a comment by ID
        public async Task<bool> DeleteCommentAsync(int id)
        {
            return await _commentRepo.DeleteAsync(id);
        }
    }
}
