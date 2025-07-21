using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViewModels
{
    public class MasjidEditViewModel : MasjidCreateViewModel
    {
        public int Id { get; set; }

        // Media management for editing
        public List<int>? MediaIdsToDelete { get; set; }  // IDs of media to update
        public List<IFormFile>? NewMediaFiles { get; set; }  // New files to upload

        // Per-language content
        public List<MasjidContentViewModel> Contents { get; set; } = new();
    }
}