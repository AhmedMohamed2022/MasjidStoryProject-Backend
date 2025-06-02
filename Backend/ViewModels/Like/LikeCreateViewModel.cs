using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel for creating a new Like
    public class LikeCreateViewModel
    {
        [Required]
        public int StoryId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
