using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }
        public bool IsApproved { get; set; }
        public string MasjidName { get; set; }
        public string AuthorFullName { get; set; }
        public string LanguageCode { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
