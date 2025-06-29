using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class StoryExtensions
    {
        public static StoryViewModel ToViewModel(this Story entity, string? currentUserId = null)
        {
            return new StoryViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                DatePublished = entity.DatePublished,
                IsApproved = entity.IsApproved,
                MasjidName = entity.Masjid?.Contents?.FirstOrDefault()?.Name ?? "Unknown",
                AuthorFullName = $"{entity.ApplicationUser?.FirstName} {entity.ApplicationUser?.LastName}",
                LanguageCode = entity.Language?.Code ?? "en",
                LikeCount = entity.Likes?.Count ?? 0,
                IsLikedByCurrentUser = currentUserId != null && entity.Likes?.Any(l => l.UserId == currentUserId) == true,
                Comments = entity.Comments?
                    .Where(c => c.IsActive)
                    .OrderByDescending(c => c.DatePosted)
                    .Select(c => c.ToViewModel())
                    .ToList() ?? new(),
                Tags = entity.StoryTags?.Select(st => st.Tag.Name).ToList() ?? new(),
                ImageUrls = entity.MediaItems?.Select(m => m.FileUrl).ToList() ?? new()
            };
        }

        public static StorySummaryViewModel ToSummaryViewModel(this Story entity)
        {
            return new StorySummaryViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                AuthorName = $"{entity.ApplicationUser?.FirstName} {entity.ApplicationUser?.LastName}",
                DatePublished = entity.DatePublished,
                LikeCount = entity.Likes?.Count ?? 0,
                CommentCount = entity.Comments?.Count ?? 0,
                ThumbnailUrl = entity.MediaItems?.FirstOrDefault()?.FileUrl ?? string.Empty
            };
        }

        public static Story ToEntity(this StoryCreateViewModel model, string userId)
        {
            return new Story
            {
                Title = model.Title,
                Content = model.Content,
                MasjidId = model.MasjidId,
                LanguageId = model.LanguageId,
                ApplicationUserId = userId,
                DatePublished = DateTime.UtcNow,
                IsApproved = false
            };
        }
        public static Story ToEntity(this StoryCreateViewModel model)
        {
            return new Story
            {
                Title = model.Title,
                Content = model.Content,
                MasjidId = model.MasjidId,
                LanguageId = model.LanguageId,
                DatePublished = DateTime.UtcNow,
                IsApproved = false
            };
        }

        public static void UpdateEntity(this StoryEditViewModel model, Story entity)
        {
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.MasjidId = model.MasjidId;
            entity.LanguageId = model.LanguageId;
            entity.IsApproved = model.IsApproved;
        }

        public static StoryEditViewModel ToEditViewModel(this Story entity)
        {
            return new StoryEditViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                MasjidId = entity.MasjidId,
                LanguageId = entity.LanguageId,
                IsApproved = entity.IsApproved,
                ExistingImageUrls = entity.MediaItems?.Select(m => m.FileUrl).ToList() ?? new()
            };
        }
    }

}
