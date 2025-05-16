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
        public Story Story { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DateLiked { get; set; } = DateTime.UtcNow;
    }
}
