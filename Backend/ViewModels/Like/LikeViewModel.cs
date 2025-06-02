using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel for displaying a Like
    public class LikeViewModel
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string UserId { get; set; }
        public DateTime DateLiked { get; set; }
    }
}

