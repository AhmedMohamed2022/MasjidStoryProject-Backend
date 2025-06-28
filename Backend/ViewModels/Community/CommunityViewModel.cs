using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CommunityViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MasjidName { get; set; }
        public int MasjidId { get; set; }
        public string LanguageCode { get; set; }
        public string CreatedByName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsUserMember { get; set; } // dynamic from service
        public int MemberCount { get; set; }
    }

}
