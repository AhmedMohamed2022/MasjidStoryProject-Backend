using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Entities;
using ViewModels;

namespace Repositories.Interfaces
{
    /// <summary>
    /// Interface for LikeRepository with ViewModel-based methods
    /// </summary>
    public interface ILikeRepository
    {
        Task<List<LikeViewModel>> GetAllAsync();
        Task<LikeViewModel?> GetByIdAsync(int id);
        Task AddAsync(LikeCreateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}

