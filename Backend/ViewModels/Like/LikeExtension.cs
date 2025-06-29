using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Entities;
using ViewModels;

namespace ViewModels
{
    /// <summary>
    /// Extension methods to convert Like entity ↔ ViewModels
    /// </summary>
    public static class LikeExtension
    {
        // Convert Like entity to LikeViewModel
        public static LikeViewModel ToViewModel(this Like entity)
        {
            return new LikeViewModel
            {
                Id = entity.Id,
                ContentId = entity.ContentId,
                ContentType = entity.ContentType,
                StoryId = entity.StoryId ?? 0, // Handle nullable StoryId
                UserId = entity.UserId,
                DateLiked = entity.DateLiked
            };
        }

        // Convert LikeCreateViewModel to Like entity
        public static Like ToEntity(this LikeCreateViewModel model)
        {
            return new Like
            {
                ContentId = model.ContentId,
                ContentType = model.ContentType,
                StoryId = model.ContentType == "Story" ? model.ContentId : null, // Set StoryId only for stories
                DateLiked = DateTime.UtcNow
            };
        }
    }
}

