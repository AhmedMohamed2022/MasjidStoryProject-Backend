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
        public DateTime EventDate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public int? MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ICollection<EventAttendee> EventAttendees { get; set; }
        public virtual ICollection<EventContent> Contents { get; set; }

    }
}
