using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class EventAttendee
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }

}
