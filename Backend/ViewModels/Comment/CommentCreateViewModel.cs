using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel used when creating a new comment
    public class CommentCreateViewModel
    {
        public int StoryId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }

}
