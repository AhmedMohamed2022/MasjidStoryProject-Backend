using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Like relationship for any content type (stories, events, communities)
    public class Like
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string ContentType { get; set; } // "Story", "Event", "Community"
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime DateLiked { get; set; } = DateTime.UtcNow;
        
        // For backward compatibility and specific relationships
        public int? StoryId { get; set; }
        public virtual Story? Story { get; set; }
    }
}
