//// Services/MediaService.cs
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Models.Entities;
//using Repositories.Interfaces;
//using ViewModels;
//using ViewModels.Media;

//namespace Services
//{
//    public class MediaService
//    {
//        private readonly IBaseRepository<Media> _mediaRepo;
//        private readonly IWebHostEnvironment _env;

//        public MediaService(IBaseRepository<Media> mediaRepo, IWebHostEnvironment env)
//        {
//            _mediaRepo = mediaRepo;
//            _env = env;
//        }

//        public async Task<bool> UploadMediaAsync(MediaCreateViewModel model)
//        {
//            // Create a wwwroot/uploads directory if it doesn't exist
//            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
//            if (!Directory.Exists(uploadsPath))
//                Directory.CreateDirectory(uploadsPath);

//            var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
//            var filePath = Path.Combine(uploadsPath, fileName);

//            // Save the file to disk
//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await model.File.CopyToAsync(stream);
//            }

//            // Save file info to DB
//            var media = new Media
//            {
//                MasjidId = model.MasjidId,
//                FileUrl = $"/uploads/{fileName}",  // relative path for frontend
//                MediaType = "Image"
//            };

//            await _mediaRepo.AddAsync(media);
//            await _mediaRepo.SaveChangesAsync();

//            return true;
//        }

//        public async Task<List<MediaViewModel>> GetMediaForMasjidAsync(int masjidId)
//        {
//            var mediaItems = await _mediaRepo.FindAsync(m => m.MasjidId == masjidId);
//            return mediaItems.Select(m => new MediaViewModel
//            {
//                Url = m.FileUrl,
//                MediaType = m.MediaType
//            }).ToList();
//        }
//        public async Task<string?> UploadUserProfilePictureAsync(IFormFile file)
//        {
//            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "profile");
//            if (!Directory.Exists(uploadsPath))
//                Directory.CreateDirectory(uploadsPath);

//            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
//            var filePath = Path.Combine(uploadsPath, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            // Return the relative URL for the frontend
//            return $"/uploads/profile/{fileName}";
//        }
//        public async Task<string?> UploadToPathAsync(IFormFile file, string folder = "general")
//        {
//            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", folder);
//            if (!Directory.Exists(uploadsRoot))
//                Directory.CreateDirectory(uploadsRoot);

//            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
//            var filePath = Path.Combine(uploadsRoot, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            // Return the relative URL
//            return $"/uploads/{folder}/{fileName}";
//        }

//    }
//}
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Hosting;
//using Models.Entities;
//using Models.Entities;
//using Repositories.Interfaces;
//using Services;
//using System.IO;
//using ViewModels;

//namespace Services
//{
//    public class MediaService 
//    {
//        private readonly IMediaRepository _repository;
//        private readonly IHostEnvironment _env;

//        public MediaService(IMediaRepository repository, IHostEnvironment env)
//        {
//            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
//            _env = env ?? throw new ArgumentNullException(nameof(env));
//        }

//        public async Task<bool> UploadMediaAsync(MediaCreateViewModel model)
//        {
//            try
//            {
//                //    if (model.File == null || model.File.Length == 0)
//                //        throw new ArgumentException("No file provided");

//                //    // Validate file size
//                //    if (model.File.Length > MaxFileSizeMB * 1024 * 1024)
//                //        throw new ArgumentException($"File size exceeds {MaxFileSizeMB}MB limit");

//                //    // Validate file extension
//                //    var extension = Path.GetExtension(model.File.FileName).ToLowerInvariant();
//                //    if (!_allowedExtensions.Contains(extension))
//                //        throw new ArgumentException($"File type not allowed. Allowed types: {string.Join(", ", _allowedExtensions)}");

//                //    // Generate unique filename
//                //    var fileName = $"{Guid.NewGuid()}{extension}";
//                //    var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

//                //    // Create uploads directory if it doesn't exist
//                //    if (!Directory.Exists(uploadsPath))
//                //        Directory.CreateDirectory(uploadsPath);

//                //    var filePath = Path.Combine(uploadsPath, fileName);

//                //    // Save file
//                //    using (var stream = new FileStream(filePath, FileMode.Create))
//                //    {
//                //        await model.File.CopyToAsync(stream);
//                //    }

//                //// Create media entity
//                //var media = new Media
//                //{
//                //    FileUrl = $"/uploads/{fileName}",
//                //    //FileName = model.File.FileName,
//                //    //FileSize = model.File.Length,
//                //    MediaType = model.File.ContentType,
//                //    MasjidId = model.MasjidId,
//                //    StoryId = model.StoryId,
//                //    DateUploaded = DateTime.UtcNow
//                //};

//                await _repository.AddAsync(model);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                // Log the error (you should use proper logging here)
//                Console.WriteLine($"Media upload failed: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<bool> DeleteMediaAsync(int id)
//        {
//            try
//            {
//                var media = await _repository.GetByIdAsync(id);
//                if (media == null) return false;

//                // Delete physical file
//                var filePath = Path.Combine(_env.ContentRootPath, media.FileUrl.TrimStart('/'));
//                if (File.Exists(filePath))
//                {
//                    File.Delete(filePath);
//                }

//                // Delete from database
//                await _repository.DeleteAsync(id);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Media deletion failed: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<List<MediaViewModel>> GetMediaByMasjidIdAsync(int masjidId)
//        {
//            return await _repository.GetByMasjidIdAsync(masjidId);
//        }

//        public async Task<List<MediaViewModel>> GetMediaByStoryIdAsync(int storyId)
//        {
//            return await _repository.GetByStoryIdAsync(storyId);
//        }
//        public async Task<string?> UploadUserProfilePictureAsync(IFormFile file)
//        {
//            var uploadsPath = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
//            if (!Directory.Exists(uploadsPath))
//                Directory.CreateDirectory(uploadsPath);

//            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
//            var filePath = Path.Combine(uploadsPath, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            // Return the relative URL for the frontend
//            return $"/uploads/profile/{fileName}";
//        }
//    }
//}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Repositories.Interfaces;
using ViewModels;

namespace Services
{
    public class MediaService 
    {
        private readonly IMediaRepository _repository;
        private readonly IWebHostEnvironment _env;
        private readonly FileProcessingService _fileProcessingService;

        public MediaService(IMediaRepository repository, IWebHostEnvironment env, FileProcessingService fileProcessingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _fileProcessingService = fileProcessingService ?? throw new ArgumentNullException(nameof(fileProcessingService));
        }

        public async Task<bool> UploadMediaAsync(MediaCreateViewModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                    throw new ArgumentException("No file provided");

                // Validate and process file using FileProcessingService
                var validationResult = _fileProcessingService.ValidateAndProcessFile(model.File);
                
                if (!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.ErrorMessage);
                }

                // Generate unique filename
                var fileName = _fileProcessingService.GenerateFileName(model.File);
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

                // Save processed file
                var filePath = await _fileProcessingService.SaveProcessedFileAsync(model.File, uploadsPath, fileName);

                // Create media entity
                var media = new Media
                {
                    FileUrl = $"/uploads/{fileName}",
                    MediaType = model.File.ContentType,
                    MasjidId = model.MasjidId,
                    StoryId = model.StoryId,
                    DateUploaded = DateTime.UtcNow
                };

                await _repository.AddAsync(model);
                
                // Log file processing results
                if (validationResult.WasCompressed)
                {
                    Console.WriteLine($"File compressed: {_fileProcessingService.GetFileSizeDisplay(validationResult.OriginalSize)} -> {_fileProcessingService.GetFileSizeDisplay(validationResult.ProcessedSize)}");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Media upload failed: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteMediaAsync(int id)
        {
            try
            {
                var media = await _repository.GetByIdAsync(id);
                if (media == null) return false;

                // Delete physical file
                var filePath = Path.Combine(_env.WebRootPath, media.FileUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Delete from database
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Media deletion failed: {ex.Message}");
                throw;
            }
        }

        public async Task<List<MediaViewModel>> GetMediaByMasjidIdAsync(int masjidId)
        {
            var mediaItems = await _repository.GetByMasjidIdAsync(masjidId);
            return mediaItems.Select(m => new MediaViewModel
            {
                Id = m.Id,
                FileUrl = m.FileUrl,
                MediaType = m.MediaType,
                UploadDate = m.UploadDate
            }).ToList();
        }

        public async Task<List<MediaViewModel>> GetMediaByStoryIdAsync(int storyId)
        {
            var mediaItems = await _repository.GetByStoryIdAsync(storyId);
            return mediaItems.Select(m => new MediaViewModel
            {
                Id = m.Id,
                FileUrl = m.FileUrl,
                MediaType = m.MediaType,
                UploadDate = m.UploadDate
            }).ToList();
        }

        public async Task<List<MediaViewModel>> GetMediaForMasjidAsync(int masjidId)
        {
            var mediaItems = await _repository.GetByMasjidIdAsync(masjidId);
            return mediaItems.Select(m => new MediaViewModel
            {
                Id = m.Id,
                FileUrl = m.FileUrl,
                MediaType = m.MediaType,
                UploadDate = m.UploadDate
            }).ToList();
        }

        public async Task<string?> UploadUserProfilePictureAsync(IFormFile file)
        {
            try
            {
                // Validate and process file
                var validationResult = _fileProcessingService.ValidateAndProcessFile(file);
                
                if (!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.ErrorMessage);
                }

                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "profile");
                var fileName = _fileProcessingService.GenerateFileName(file);

                // Save processed file
                await _fileProcessingService.SaveProcessedFileAsync(file, uploadsPath, fileName);

                // Return the relative URL for the frontend
                return $"/uploads/profile/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Profile picture upload failed: {ex.Message}");
                throw;
            }
        }

        public async Task<string?> UploadToPathAsync(IFormFile file, string folder = "general")
        {
            try
            {
                // Validate and process file
                var validationResult = _fileProcessingService.ValidateAndProcessFile(file);
                
                if (!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.ErrorMessage);
                }

                var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", folder);
                var fileName = _fileProcessingService.GenerateFileName(file);

                // Save processed file
                await _fileProcessingService.SaveProcessedFileAsync(file, uploadsRoot, fileName);

                // Return the relative URL
                return $"/uploads/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File upload failed: {ex.Message}");
                throw;
            }
        }
    }
}