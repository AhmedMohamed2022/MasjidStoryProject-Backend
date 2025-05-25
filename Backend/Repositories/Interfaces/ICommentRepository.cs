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
        Task<List<CommentViewModel>> GetAllAsync();
        Task<CommentViewModel?> GetByIdAsync(int id);
        Task<CommentEditViewModel?> GetEditByIdAsync(int id);
        Task AddAsync(CommentCreateViewModel model);
        Task<bool> UpdateAsync(CommentEditViewModel model);
        Task<bool> DeleteAsync(int id);
    }

}
