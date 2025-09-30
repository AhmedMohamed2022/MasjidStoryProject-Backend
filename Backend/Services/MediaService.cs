using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Repositories.Interfaces;
using ViewModels;
using MasjidStory.Services;

namespace Services
{
    public class MediaService 
    {
        private readonly IMediaRepository _repository;
        private readonly IBaseRepository<Media> _baseRepo;
        private readonly IWebHostEnvironment _env;
        private readonly FileProcessingService _fileProcessingService;
        private readonly ImageService _imageService;

        public MediaService(IMediaRepository repository, IBaseRepository<Media> baseRepo, IWebHostEnvironment env, FileProcessingService fileProcessingService, ImageService imageService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _baseRepo = baseRepo ?? throw new ArgumentNullException(nameof(baseRepo));
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _fileProcessingService = fileProcessingService ?? throw new ArgumentNullException(nameof(fileProcessingService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
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

                // Upload to Cloudinary
                string cloudinaryUrl;
                using (var stream = model.File.OpenReadStream())
                {
                    cloudinaryUrl = await _imageService.UploadImageAsync(stream, model.File.FileName);
                }

                // Create media entity
                var media = new Media
                {
                    FileUrl = cloudinaryUrl,
                    MediaType = model.File.ContentType,
                    MasjidId = model.MasjidId,
                    StoryId = model.StoryId,
                    DateUploaded = DateTime.UtcNow
                };

                await _baseRepo.AddAsync(media);
                await _baseRepo.SaveChangesAsync();

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
                // The original FileUrl was a local path, so we can't delete it directly here
                // if it was uploaded to Cloudinary.
                // For now, we'll just remove it from the DB.
                // If it was Cloudinary, we'd need to delete from Cloudinary.
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

                string cloudinaryUrl;
                using (var stream = file.OpenReadStream())
                {
                    cloudinaryUrl = await _imageService.UploadImageAsync(stream, file.FileName);
                }

                // Return the relative URL for the frontend
                return cloudinaryUrl;
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

                string cloudinaryUrl;
                using (var stream = file.OpenReadStream())
                {
                    cloudinaryUrl = await _imageService.UploadImageAsync(stream, file.FileName);
                }

                // Return the relative URL
                return cloudinaryUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File upload failed: {ex.Message}");
                throw;
            }
        }
    }
}