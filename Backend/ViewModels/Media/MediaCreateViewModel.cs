using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Media
{
    public class MediaCreateViewModel
    {
        [Required]
        public int MasjidId { get; set; }

        [Required]
        public IFormFile File { get; set; }  // For actual upload
    }

}
