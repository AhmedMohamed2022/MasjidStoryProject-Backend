using Microsoft.AspNetCore.Http;

namespace ViewModels
{
    public class MediaCreateViewModel
    {
        public IFormFile File { get; set; }
        public int? MasjidId { get; set; }
        public int? StoryId { get; set; }
    }
}