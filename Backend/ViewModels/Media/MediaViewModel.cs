﻿namespace ViewModels
{
    public class MediaViewModel
    {
        public int Id { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string? MediaType { get; set; } // Assuming MediaType is a string, adjust as necessary
        public int? MasjidId { get; set; }
        public int? StoryId { get; set; }
        public DateTime UploadDate { get; set; }
    }
}