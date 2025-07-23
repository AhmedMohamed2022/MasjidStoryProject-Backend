using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Story entity for user-submitted content
    public class Story
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<StoryTag> StoryTags { get; set; }
        public virtual ICollection<Media> MediaItems { get; set; } = new List<Media>();
        // Add translation support
        public virtual ICollection<StoryContent> Contents { get; set; }
    }
}
