using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class CommunityMember
    {
        public int CommunityId { get; set; }
        public Community Community { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }
}
