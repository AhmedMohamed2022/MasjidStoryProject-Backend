using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Event entity for mosque-related events
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public int? LanguageId { get; set; }
        public Language Language { get; set; }
        public int? MasjidId { get; set; }
        public Masjid Masjid { get; set; }
        public ICollection<EventAttendee> EventAttendees { get; set; }
    }
}
