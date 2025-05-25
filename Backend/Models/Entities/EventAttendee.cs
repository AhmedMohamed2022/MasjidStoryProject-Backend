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
        public virtual Event Event { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }

}
