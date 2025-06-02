using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel used when creating a new comment
    public class CommentCreateViewModel
    {
        [Required]
        public int StoryId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }


}
