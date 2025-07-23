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
        public string LocalizedTitle { get; set; }
        public string LocalizedContent { get; set; }
        public DateTime DatePublished { get; set; }
        public bool IsApproved { get; set; }

        public string MasjidName { get; set; }
        public string AuthorFullName { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> ImageUrls { get; set; } = new();
        public List<MediaViewModel> MediaItems { get; set; } = new();
        public List<StoryContentViewModel> Contents { get; set; } = new();
    }

}
