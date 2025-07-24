using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Community groups for masjids
    public class Community
    {
        public int Id { get; set; }
        public int MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ICollection<CommunityMember> CommunityMembers { get; set; }
        // Multilingual content
        public virtual ICollection<CommunityContent> Contents { get; set; }
    }
}
