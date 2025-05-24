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
        public string Title { get; set; }
        public string Content { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ICollection<CommunityMember> CommunityMembers { get; set; }
    }
}
