using System;
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

        public async Task<bool> ToggleLikeAsync(int contentId, string contentType, string userId)
        {
            var existing = await _baseRepo.FindAsync(l => l.ContentId == contentId && l.ContentType == contentType && l.UserId == userId);
            var like = existing.FirstOrDefault();

            if (like != null)
            {
                _baseRepo.Delete(like);
            }
            else
            {
                await _baseRepo.AddAsync(new Like
                {
                    ContentId = contentId,
                    ContentType = contentType,
                    StoryId = contentType == "Story" ? contentId : null, // Set StoryId only for stories
                    UserId = userId,
                    DateLiked = DateTime.UtcNow
                });
            }

            await _baseRepo.SaveChangesAsync();
            return like == null; // true if added
        }

        public async Task<int> GetLikeCountAsync(int contentId, string contentType)
        {
            var likes = await _baseRepo.FindAsync(l => l.ContentId == contentId && l.ContentType == contentType);
            return likes.Count();
        }

        public async Task<bool> IsLikedByUserAsync(int contentId, string contentType, string userId)
        {
            var like = await _baseRepo.GetFirstOrDefaultAsync(l => l.ContentId == contentId && l.ContentType == contentType && l.UserId == userId);
            return like != null;
        }
    }
}
