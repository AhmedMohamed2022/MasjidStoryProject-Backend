using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StorySummaryViewModel
    {
        public int Id { get; set; }
        public string LocalizedTitle { get; set; }
        public string AuthorName { get; set; }
        public DateTime DatePublished { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;

    }

}
