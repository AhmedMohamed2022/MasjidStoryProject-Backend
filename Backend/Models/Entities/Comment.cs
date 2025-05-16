using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // User comments on stories
    public class Comment
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public Story Story { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
