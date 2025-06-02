using Repositories.Interfaces;
using ViewModels;

namespace Services
{
    /// <summary>
    /// Service layer for handling business logic related to Likes
    /// </summary>
    public class LikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        /// <summary>
        /// Returns all likes
        /// </summary>
        public async Task<List<LikeViewModel>> GetAllLikesAsync()
        {
            return await _likeRepository.GetAllAsync();
        }

        /// <summary>
        /// Returns a like by its ID
        /// </summary>
        public async Task<LikeViewModel?> GetLikeByIdAsync(int id)
        {
            return await _likeRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new like
        /// </summary>
        public async Task AddLikeAsync(LikeCreateViewModel model)
        {
            await _likeRepository.AddAsync(model);
        }

        /// <summary>
        /// Deletes a like by ID
        /// </summary>
        public async Task<bool> DeleteLikeAsync(int id)
        {
            return await _likeRepository.DeleteAsync(id);
        }
    }
}
