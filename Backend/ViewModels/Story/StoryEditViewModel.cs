using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StoryEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int MasjidId { get; set; }

        public int? LanguageId { get; set; }

        public bool IsApproved { get; set; }
        
        // New images to upload
        public List<IFormFile>? NewStoryImages { get; set; }
        
        // Media IDs to keep (all other media will be deleted)
        public List<int>? KeepMediaIds { get; set; }
        
        // Media IDs to specifically remove
        public List<int>? RemoveMediaIds { get; set; }

        // Content change tracking
        public string? OriginalTitle { get; set; }
        public string? OriginalContent { get; set; }
        public bool RequiresReapproval { get; set; } = false;
        public string? ChangeReason { get; set; }
    }
}
