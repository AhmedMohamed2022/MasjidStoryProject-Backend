using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories.Interfaces
{

    /// <summary>
    /// Interface for comment-specific data operations
    /// </summary>
    public interface ICommentRepository
    {
        Task<CommentViewModel> AddAsync(CommentCreateViewModel model, string userId);
        Task<List<CommentViewModel>> GetByStoryIdAsync(int storyId);
        Task<List<CommentViewModel>> GetAllAsync(); // keep for admin if needed
        Task<CommentViewModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(CommentEditViewModel model);
        Task<bool> DeleteAsync(int id);

    }

}
