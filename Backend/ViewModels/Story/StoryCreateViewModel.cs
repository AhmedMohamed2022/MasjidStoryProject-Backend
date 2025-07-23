using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StoryCreateViewModel
    {
        [Required]
        public int MasjidId { get; set; }
        public List<StoryContentViewModel> Contents { get; set; } = new();
        public List<string>? Tags { get; set; }
        public List<IFormFile>? StoryImages { get; set; }
    }

}
