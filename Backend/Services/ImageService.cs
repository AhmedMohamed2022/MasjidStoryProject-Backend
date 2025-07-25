using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MasjidStory.Services
{
    public class ImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];
            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl?.ToString();
        }

        // For future use: video upload
        public async Task<string> UploadVideoAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl?.ToString();
        }
    }
} 