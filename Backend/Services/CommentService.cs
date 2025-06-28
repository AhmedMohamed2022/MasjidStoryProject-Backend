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
        private readonly ICommentRepository _repo;

        public CommentService(ICommentRepository repo)
        {
            _repo = repo;
        }

        public async Task<CommentViewModel> AddCommentAsync(CommentCreateViewModel model, string userId)
        {
            return await _repo.AddAsync(model, userId);
        }


        public async Task<List<CommentViewModel>> GetCommentsByStoryAsync(int storyId)
        {
            return await _repo.GetByStoryIdAsync(storyId);
        }

        public async Task<List<CommentViewModel>> GetAllCommentsAsync() => await _repo.GetAllAsync();
        public async Task<CommentViewModel?> GetCommentByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<bool> UpdateCommentAsync(CommentEditViewModel model) => await _repo.UpdateAsync(model);
        public async Task<bool> DeleteCommentAsync(int id) => await _repo.DeleteAsync(id);
    }

}
