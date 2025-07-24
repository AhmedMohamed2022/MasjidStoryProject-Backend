using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Localized content for communities
    public class CommunityContent
    {
        public int Id { get; set; }
        public int CommunityId { get; set; }
        public virtual Community Community { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
} 