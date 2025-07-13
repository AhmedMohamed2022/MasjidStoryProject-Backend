using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;

namespace Services
{
    public class FileProcessingService
    {
        private const int MaxFileSizeMB = 10; // 10MB max file size
        private const int MaxImageDimension = 2048; // Max width/height
        private const int Quality = 85; // JPEG quality
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

        public class FileValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
            public long OriginalSize { get; set; }
            public long ProcessedSize { get; set; }
            public bool WasCompressed { get; set; }
        }

        public FileValidationResult ValidateAndProcessFile(IFormFile file)
        {
            var result = new FileValidationResult
            {
                OriginalSize = file.Length
            };

            try
            {
                // Check if file is null or empty
                if (file == null || file.Length == 0)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "No file provided";
                    return result;
                }

                // Check file size
                if (file.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"File size ({file.Length / (1024 * 1024)}MB) exceeds the maximum allowed size of {MaxFileSizeMB}MB";
                    return result;
                }

                // Check file extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", AllowedExtensions)}";
                    return result;
                }

                // Check MIME type
                if (!AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"File MIME type '{file.ContentType}' is not allowed";
                    return result;
                }

                // Process and compress image if needed
                var processedStream = ProcessImage(file);
                result.ProcessedSize = processedStream.Length;
                result.WasCompressed = processedStream.Length < file.Length;
                result.IsValid = true;

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"File processing failed: {ex.Message}";
                return result;
            }
        }

        private MemoryStream ProcessImage(IFormFile file)
        {
            var outputStream = new MemoryStream();

            try
            {
                using var image = Image.Load(file.OpenReadStream());
                
                // Check if image needs resizing
                bool needsResize = image.Width > MaxImageDimension || image.Height > MaxImageDimension;
                
                if (needsResize)
                {
                    // Calculate new dimensions maintaining aspect ratio
                    var ratio = Math.Min((double)MaxImageDimension / image.Width, (double)MaxImageDimension / image.Height);
                    var newWidth = (int)(image.Width * ratio);
                    var newHeight = (int)(image.Height * ratio);

                    image.Mutate(x => x.Resize(newWidth, newHeight));
                }

                // Determine output format and save
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        image.Save(outputStream, new JpegEncoder { Quality = Quality });
                        break;
                    case ".png":
                        image.Save(outputStream, new PngEncoder());
                        break;
                    case ".gif":
                        image.Save(outputStream, new GifEncoder());
                        break;
                    case ".webp":
                        image.Save(outputStream, new WebpEncoder());
                        break;
                    default:
                        // Default to JPEG
                        image.Save(outputStream, new JpegEncoder { Quality = Quality });
                        break;
                }

                outputStream.Position = 0;
                return outputStream;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process image: {ex.Message}");
            }
        }

        public async Task<string> SaveProcessedFileAsync(IFormFile file, string uploadPath, string fileName)
        {
            var validationResult = ValidateAndProcessFile(file);
            
            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            // If file was processed/compressed, save the processed version
            if (validationResult.WasCompressed)
            {
                using var processedStream = ProcessImage(file);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await processedStream.CopyToAsync(fileStream);
            }
            else
            {
                // Save original file if no processing was needed
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }

        public string GenerateFileName(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return $"{Guid.NewGuid()}{extension}";
        }

        public string GetFileSizeDisplay(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
} 