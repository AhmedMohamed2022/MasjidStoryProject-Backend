using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel to represent a comment when displaying to the client
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsActive { get; set; }
    }
}
