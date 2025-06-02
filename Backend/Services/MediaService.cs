// Services/MediaService.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Repositories.Interfaces;
using ViewModels;
using ViewModels.Media;

namespace Services
{
    public class MediaService
    {
        private readonly IBaseRepository<Media> _mediaRepo;
        private readonly IWebHostEnvironment _env;

        public MediaService(IBaseRepository<Media> mediaRepo, IWebHostEnvironment env)
        {
            _mediaRepo = mediaRepo;
            _env = env;
        }

        public async Task<bool> UploadMediaAsync(MediaCreateViewModel model)
        {
            // Create a wwwroot/uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save file info to DB
            var media = new Media
            {
                MasjidId = model.MasjidId,
                FileUrl = $"/uploads/{fileName}",  // relative path for frontend
                MediaType = "Image"
            };

            await _mediaRepo.AddAsync(media);
            await _mediaRepo.SaveChangesAsync();

            return true;
        }

        public async Task<List<MediaViewModel>> GetMediaForMasjidAsync(int masjidId)
        {
            var mediaItems = await _mediaRepo.FindAsync(m => m.MasjidId == masjidId);
            return mediaItems.Select(m => new MediaViewModel
            {
                Url = m.FileUrl,
                MediaType = m.MediaType
            }).ToList();
        }
    }
}
