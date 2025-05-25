using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Like relationship for stories
    public class Like
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime DateLiked { get; set; } = DateTime.UtcNow;
    }
}
