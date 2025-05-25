using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class CommunityMember
    {
        public int Id { get; set; } //  surrogate key

        public int CommunityId { get; set; }
        public virtual Community Community { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }
}
