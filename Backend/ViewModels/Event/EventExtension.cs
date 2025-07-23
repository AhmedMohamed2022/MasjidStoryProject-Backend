using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using ViewModels;
namespace ViewModels
{
    public static class EventExtension
    {
        public static EventViewModel ToViewModel(this Event e, string languageCode = "en")
        {
            int langId = languageCode.ToLower() == "ar" ? 2 : 1;
            var localizedContent = e.Contents?.FirstOrDefault(c => c.LanguageId == langId)
                ?? e.Contents?.FirstOrDefault(c => c.LanguageId == 1)
                ?? e.Contents?.FirstOrDefault();
            return new EventViewModel
            {
                Id = e.Id,
                LocalizedTitle = localizedContent?.Title ?? string.Empty,
                LocalizedDescription = localizedContent?.Description ?? string.Empty,
                EventDate = e.EventDate,
                MasjidId = e.MasjidId,
                MasjidName = e.Masjid?.Contents?.FirstOrDefault(c => c.LanguageId == langId)?.Name ?? e.Masjid?.Contents?.FirstOrDefault()?.Name,
                CreatedByName = $"{e.CreatedBy?.FirstName} {e.CreatedBy?.LastName}",
                CreatedById = e.CreatedById,
                Contents = e.Contents?.Select(c => new EventContentViewModel {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Description = c.Description
                }).ToList() ?? new List<EventContentViewModel>()
            };
        }

        public static Event ToEntity(this EventCreateViewModel model, string userId)
        {
            return new Event
            {
                EventDate = model.EventDate,
                MasjidId = model.MasjidId,
                CreatedById = userId,
                DateCreated = DateTime.UtcNow,
                Contents = model.Contents?.Select(c => new EventContent {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Description = c.Description
                }).ToList() ?? new List<EventContent>()
            };
        }
        public static EventViewModel ToViewModel(this Event e, string? userId, string languageCode = "en")
        {
            int langId = languageCode.ToLower() == "ar" ? 2 : 1;
            var localizedContent = e.Contents?.FirstOrDefault(c => c.LanguageId == langId)
                ?? e.Contents?.FirstOrDefault(c => c.LanguageId == 1)
                ?? e.Contents?.FirstOrDefault();
            return new EventViewModel
            {
                Id = e.Id,
                LocalizedTitle = localizedContent?.Title ?? string.Empty,
                LocalizedDescription = localizedContent?.Description ?? string.Empty,
                EventDate = e.EventDate,
                MasjidId = e.MasjidId,
                MasjidName = e.Masjid?.Contents?.FirstOrDefault(c => c.LanguageId == langId)?.Name ?? e.Masjid?.Contents?.FirstOrDefault()?.Name,
                CreatedByName = $"{e.CreatedBy?.FirstName} {e.CreatedBy?.LastName}",
                CreatedById = e.CreatedById,
                IsUserRegistered = userId != null && e.EventAttendees?.Any(a => a.UserId == userId) == true,
                Contents = e.Contents?.Select(c => new EventContentViewModel {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Description = c.Description
                }).ToList() ?? new List<EventContentViewModel>()
            };
        }

    }
}
