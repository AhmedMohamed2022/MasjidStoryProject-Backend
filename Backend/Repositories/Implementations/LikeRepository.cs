using Models.Entities;
using Repositories.Interfaces;
using ViewModels;

namespace Repositories.Implementations
{
    /// <summary>
    /// Repository for managing Like entities using ViewModels
    /// </summary>
    public class LikeRepository : ILikeRepository
    {
        private readonly IBaseRepository<Like> _baseRepo;

        public LikeRepository(IBaseRepository<Like> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        //// Fetch all Likes and map them to LikeViewModel
        //public async Task<List<LikeViewModel>> GetAllAsync()
        //{
        //    var likes = await _baseRepo.GetAllAsync();
        //    return likes.Select(l => l.ToViewModel()).ToList();
        //}

        //// Fetch Like by ID with ViewModel mapping
        //public async Task<LikeViewModel?> GetByIdAsync(int id)
        //{
        //    var entity = await _baseRepo.GetByIdAsync(id);
        //    return entity?.ToViewModel();
        //}

        //// Add a new Like using a ViewModel
        //public async Task AddAsync(LikeCreateViewModel model)
        //{
        //    var entity = model.ToEntity();
        //    await _baseRepo.AddAsync(entity);
        //    await _baseRepo.SaveChangesAsync();
        //}

        //// Delete a Like by ID
        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var entity = await _baseRepo.GetByIdAsync(id);
        //    if (entity == null)
        //        return false;

        //    _baseRepo.Delete(entity);
        //    await _baseRepo.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> ToggleLikeAsync(int storyId, string userId)
        {
            var existing = await _baseRepo.FindAsync(l => l.StoryId == storyId && l.UserId == userId);
            var like = existing.FirstOrDefault();

            if (like != null)
            {
                _baseRepo.Delete(like);
            }
            else
            {
                await _baseRepo.AddAsync(new Like
                {
                    StoryId = storyId,
                    UserId = userId,
                    DateLiked = DateTime.UtcNow
                });
            }

            await _baseRepo.SaveChangesAsync();
            return like == null; // true if added
        }

    }
}
