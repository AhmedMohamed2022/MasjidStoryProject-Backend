using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Blog posts
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
