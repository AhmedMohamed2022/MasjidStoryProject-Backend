using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Media items (images/videos) for masjids or stories
    public class Media
    {
        public int Id { get; set; }

        // Optional relation: Masjid OR Story
        public int? MasjidId { get; set; }
        public virtual Masjid? Masjid { get; set; }

        public int? StoryId { get; set; }
        public virtual Story? Story { get; set; }

        public string FileUrl { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MediaType { get; set; } = "Image";
        public string ContentType { get; set; } = "image/jpeg"; // Default to JPEG, can be overridden
        public DateTime DateUploaded { get; set; } = DateTime.UtcNow;
        
    }

}
