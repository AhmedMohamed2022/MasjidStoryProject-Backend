using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
namespace ViewModels
{
    public static class EventExtension
    {
        public static EventViewModel ToViewModel(this Event e)
        {
            return new EventViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                MasjidName = e.Masjid?.ShortName ?? "General"
            };
        }

        public static Event ToEntity(this EventCreateViewModel model, string userId)
        {
            return new Event
            {
                Title = model.Title,
                Description = model.Description,
                EventDate = model.EventDate,
                MasjidId = model.MasjidId,
                LanguageId = model.LanguageId,
                CreatedById = userId,
                DateCreated = DateTime.UtcNow
            };
        }
        public static EventViewModel ToViewModel(this Event e, string? userId = null)
        {
            return new EventViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                MasjidId = e.MasjidId,
                MasjidName = e.Masjid?.ShortName ?? "General",
                CreatedByName = $"{e.CreatedBy?.FirstName} {e.CreatedBy?.LastName}",
                IsUserRegistered = userId != null && e.EventAttendees?.Any(a => a.UserId == userId) == true
            };
        }

    }
}
