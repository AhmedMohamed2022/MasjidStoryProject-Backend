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
        public int CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}
