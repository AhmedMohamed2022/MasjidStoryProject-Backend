using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // User comments on any content type (stories, events, communities)
    public class Comment
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string ContentType { get; set; } // "Story", "Event", "Community"
        public string UserId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // For backward compatibility and specific relationships
        public int? StoryId { get; set; }
        public virtual Story? Story { get; set; }
    }
}
