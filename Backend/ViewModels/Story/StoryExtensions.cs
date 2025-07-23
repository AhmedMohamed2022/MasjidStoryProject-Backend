using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using ViewModels;
namespace ViewModels
{
    public static class StoryExtensions
    {
        public static StoryViewModel ToViewModel(this Story entity, string? currentUserId = null, string languageCode = "en")
        {
            var content = entity.Contents?.FirstOrDefault(c => c.Language != null && c.Language.Code == languageCode)
                ?? entity.Contents?.FirstOrDefault();
            return new StoryViewModel
            {
                Id = entity.Id,
                LocalizedTitle = content?.Title ?? string.Empty,
                LocalizedContent = content?.Content ?? string.Empty,
                DatePublished = entity.DatePublished,
                IsApproved = entity.IsApproved,
                MasjidName = entity.Masjid?.Contents?.FirstOrDefault()?.Name ?? "Unknown",
                AuthorFullName = $"{entity.ApplicationUser?.FirstName} {entity.ApplicationUser?.LastName}",
                LikeCount = entity.Likes?.Count ?? 0,
                IsLikedByCurrentUser = currentUserId != null && entity.Likes?.Any(l => l.UserId == currentUserId) == true,
                Comments = entity.Comments?
                    .Where(c => c.IsActive)
                    .OrderByDescending(c => c.DatePosted)
                    .Select(c => c.ToViewModel())
                    .ToList() ?? new(),
                Tags = entity.StoryTags?.Select(st =>
                {
                    var tagContent = st.Tag.Contents?.FirstOrDefault(tc => tc.Language != null && tc.Language.Code == languageCode)
                        ?? st.Tag.Contents?.FirstOrDefault();
                    return tagContent?.Name ?? "";
                }).ToList() ?? new(),
                ImageUrls = entity.MediaItems?.Select(m => m.FileUrl).ToList() ?? new(),
                MediaItems = entity.MediaItems?.Select(m => m.ToViewModel()).ToList() ?? new(),
                Contents = entity.Contents?.Select(c => new StoryContentViewModel
                {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? new()
            };
        }

        public static StorySummaryViewModel ToSummaryViewModel(this Story entity, string languageCode = "en")
        {
            var content = entity.Contents?.FirstOrDefault(c => c.Language != null && c.Language.Code == languageCode)
                ?? entity.Contents?.FirstOrDefault();
            return new StorySummaryViewModel
            {
                Id = entity.Id,
                LocalizedTitle = content?.Title ?? string.Empty,
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
                MasjidId = model.MasjidId,
                ApplicationUserId = userId,
                DatePublished = DateTime.UtcNow,
                IsApproved = false,
                Contents = model.Contents?.Select(c => new StoryContent
                {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? new()
            };
        }
        public static Story ToEntity(this StoryCreateViewModel model)
        {
            return new Story
            {
                MasjidId = model.MasjidId,
                DatePublished = DateTime.UtcNow,
                IsApproved = false,
                Contents = model.Contents?.Select(c => new StoryContent
                {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? new()
            };
        }

        public static void UpdateEntity(this StoryEditViewModel model, Story entity)
        {
            entity.MasjidId = model.MasjidId;
            entity.IsApproved = model.IsApproved;
            // Update translations
            if (model.Contents != null)
            {
                entity.Contents.Clear();
                foreach (var c in model.Contents)
                {
                    entity.Contents.Add(new StoryContent
                    {
                        LanguageId = c.LanguageId,
                        Title = c.Title,
                        Content = c.Content
                    });
                }
            }
            // Add audit trail information (you might want to add these fields to the Story entity)
            // entity.LastModified = DateTime.UtcNow;
            // entity.RequiresReapproval = model.RequiresReapproval;
            // entity.ChangeReason = model.ChangeReason;
        }

        //public static StoryEditViewModel ToEditViewModel(this Story entity)
        //{
        //    return new StoryEditViewModel
        //    {
        //        Id = entity.Id,
        //        Title = entity.Title,
        //        Content = entity.Content,
        //        MasjidId = entity.MasjidId,
        //        LanguageId = entity.LanguageId,
        //        IsApproved = entity.IsApproved,
        //        ExistingImageUrls = entity.MediaItems?.Select(m => m.FileUrl).ToList() ?? new()
        //    };
        //}
    }

}
