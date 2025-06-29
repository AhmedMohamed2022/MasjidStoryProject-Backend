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
        public int ContentId { get; set; }
        
        [Required]
        public string ContentType { get; set; } // "Story", "Event", "Community"

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
