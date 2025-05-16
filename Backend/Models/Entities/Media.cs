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
        public int MasjidId { get; set; }
        public Masjid Masjid { get; set; }
        public string FileUrl { get; set; }
        public string MediaType { get; set; } = "Image";
        public DateTime DateUploaded { get; set; } = DateTime.UtcNow;
    }
}
