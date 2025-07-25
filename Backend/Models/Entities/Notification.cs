using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{

    // In-app notifications
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string MessageKey { get; set; } // e.g., NOTIFICATION.LIKE
        public string MessageVariables { get; set; } // JSON string of variables for interpolation
        public string Type { get; set; }
        public string ContentType { get; set; }
        public int? ContentId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public string SenderName { get; set; }
    }
}
