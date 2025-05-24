using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // User visits to masjids
    public class MasjidVisit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
    }
}
