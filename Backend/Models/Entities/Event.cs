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
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public int? LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public int? MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public virtual ICollection<EventAttendee> EventAttendees { get; set; }
    }
}
