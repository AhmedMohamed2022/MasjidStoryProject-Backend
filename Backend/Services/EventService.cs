using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class EventService
    {
        private readonly IBaseRepository<Event> _baseRepo;
        private readonly IBaseRepository<EventAttendee> _attendeeRepo;

        public EventService(IBaseRepository<Event> baseRepo, IBaseRepository<EventAttendee> attendeeRepo)
        {
            _baseRepo = baseRepo;
            _attendeeRepo = attendeeRepo;
        }

        public async Task<List<EventViewModel>> GetUpcomingEventsAsync(string languageCode = "en")
        {
            var events = await _baseRepo.FindAsync(
                e => e.EventDate >= DateTime.UtcNow,
                e => e.Masjid,
                e => e.CreatedBy,
                e => e.Contents
            );

            return events
                .OrderBy(e => e.EventDate)
                .Take(6)
                .Select(e => e.ToViewModel(languageCode))
                .ToList();
        }

        public async Task AddEventAsync(EventCreateViewModel model, string userId)
        {
            var entity = model.ToEntity(userId);
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();
        }

        public async Task<bool> RegisterUserToEventAsync(int eventId, string userId)
        {
            var alreadyExists = await _attendeeRepo.AnyAsync(
                ea => ea.EventId == eventId && ea.UserId == userId
            );

            if (alreadyExists) return false;

            await _attendeeRepo.AddAsync(new EventAttendee
            {
                EventId = eventId,
                UserId = userId,
                RegistrationDate = DateTime.UtcNow
            });

            await _attendeeRepo.SaveChangesAsync();
            return true;
        }
        public async Task<EventViewModel?> GetEventDetailsAsync(int id, string? userId, string languageCode = "en")
        {
            var ev = await _baseRepo.GetFirstOrDefaultAsync(
                e => e.Id == id,
                e => e.Masjid,
                e => e.CreatedBy,
                e => e.EventAttendees,
                e => e.Contents
            );

            return ev?.ToViewModel(userId, languageCode);
        }

        public async Task<List<EventViewModel>> GetMasjidEventsAsync(int masjidId, string languageCode = "en")
        {
            var events = await _baseRepo.FindAsync(
                e => e.MasjidId == masjidId && e.EventDate >= DateTime.UtcNow,
                e => e.Masjid,
                e => e.CreatedBy,
                e => e.Contents
            );

            return events.Select(e => e.ToViewModel(languageCode)).ToList();
        }

        public async Task<List<EventViewModel>> GetUserRegisteredEventsAsync(string userId, string languageCode = "en")
        {
            var attendeeRecords = await _attendeeRepo.FindAsync(a => a.UserId == userId, a => a.Event);

            var events = attendeeRecords
                .Select(a => a.Event)
                .Where(e => e != null)
                .Distinct()
                .ToList();

            // Load CreatedBy for each event
            foreach (var ev in events)
            {
                if (ev.CreatedById != null)
                {
                    // Load the CreatedBy navigation property
                    await _baseRepo.GetFirstOrDefaultAsync(e => e.Id == ev.Id, e => e.CreatedBy);
                }
            }

            return events.Select(e => e.ToViewModel(userId, languageCode)).ToList();
        }

        public async Task<bool> UpdateEventAsync(int id, EventCreateViewModel model, string userId)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null || (entity.CreatedById != userId)) return false;

            // Remove old contents and add new ones
            entity.Contents?.Clear();
            if (model.Contents != null)
            {
                entity.Contents = model.Contents.Select(c => new EventContent
                {
                    LanguageId = c.LanguageId,
                    Title = c.Title,
                    Description = c.Description
                }).ToList();
            }
            entity.EventDate = model.EventDate;
            entity.MasjidId = model.MasjidId;

            _baseRepo.Update(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventAsync(int id, string userId)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null || entity.CreatedById != userId) return false;

            _baseRepo.Delete(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

    }

}
