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
        public static StoryViewModel ToViewModel(this Models.Entities.Story entity)
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
                CommentCount = entity.Comments?.Count ?? 0
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
                IsApproved = false // pending approval
            };
        }

        public static void UpdateEntity(this StoryEditViewModel vm, Models.Entities.Story entity)
        {
            entity.Title = vm.Title;
            entity.Content = vm.Content;
            entity.MasjidId = vm.MasjidId;
            entity.LanguageId = vm.LanguageId;
            entity.IsApproved = vm.IsApproved;
        }

        public static StoryEditViewModel ToEditViewModel(this Models.Entities.Story entity)
        {
            return new StoryEditViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                MasjidId = entity.MasjidId,
                LanguageId = entity.LanguageId,
                IsApproved = entity.IsApproved
            };
        }
    }
}
