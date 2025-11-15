using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileUploadService> _logger;
        private readonly string _uploadPath;
        private readonly long _maxFileSize;
        private readonly string[] _allowedImageTypes;
        private readonly string[] _allowedDocumentTypes;

        public FileUploadService(IConfiguration configuration, ILogger<FileUploadService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            _uploadPath = _configuration["FileUpload:UploadPath"] ?? "uploads";
            
            var maxFileSizeStr = _configuration["FileUpload:MaxFileSize"];
            _maxFileSize = long.TryParse(maxFileSizeStr, out var maxSize) ? maxSize : 10485760; // 10MB default
            
            var imageTypesSection = _configuration.GetSection("FileUpload:AllowedImageTypes");
            _allowedImageTypes = imageTypesSection.Exists() && imageTypesSection.GetChildren().Any()
                ? imageTypesSection.GetChildren().Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x)).ToArray()
                : new[] { "jpg", "jpeg", "png", "gif", "webp" };
            
            var docTypesSection = _configuration.GetSection("FileUpload:AllowedDocumentTypes");
            _allowedDocumentTypes = docTypesSection.Exists() && docTypesSection.GetChildren().Any()
                ? docTypesSection.GetChildren().Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x)).ToArray()
                : new[] { "pdf", "doc", "docx" };
            
            // Ensure upload directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<FileUploadResult> UploadImageAsync(Stream fileStream, string fileName, long fileSize)
        {
            return await UploadFileAsync(fileStream, fileName, fileSize, _allowedImageTypes, "images");
        }

        public async Task<FileUploadResult> UploadDocumentAsync(Stream fileStream, string fileName, long fileSize)
        {
            return await UploadFileAsync(fileStream, fileName, fileSize, _allowedDocumentTypes, "documents");
        }

        private async Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, long fileSize, string[] allowedTypes, string subfolder)
        {
            if (fileStream == null || fileSize == 0)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = "No file provided"
                };
            }

            if (fileSize > _maxFileSize)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"File size exceeds maximum allowed size of {_maxFileSize / 1024 / 1024}MB"
                };
            }

            var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
            if (!allowedTypes.Contains(fileExtension))
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"File type '{fileExtension}' is not allowed. Allowed types: {string.Join(", ", allowedTypes)}"
                };
            }

            try
            {
                var folderPath = Path.Combine(_uploadPath, subfolder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/{subfolder}/{uniqueFileName}";

                _logger.LogInformation("File uploaded successfully: {FileName} to {FilePath}", uniqueFileName, filePath);

                return new FileUploadResult
                {
                    Success = true,
                    FileUrl = fileUrl,
                    FileName = uniqueFileName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {FileName}", fileName);
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while uploading the file"
                };
            }
        }

        public async Task<bool> DeleteFileAsync(string filename)
        {
            try
            {
                // Try to find the file in both images and documents folders
                var imagePath = Path.Combine(_uploadPath, "images", filename);
                var documentPath = Path.Combine(_uploadPath, "documents", filename);

                string filePath = null;
                if (File.Exists(imagePath))
                {
                    filePath = imagePath;
                }
                else if (File.Exists(documentPath))
                {
                    filePath = documentPath;
                }

                if (filePath == null)
                {
                    _logger.LogWarning("File not found for deletion: {FileName}", filename);
                    return false;
                }

                File.Delete(filePath);
                _logger.LogInformation("File deleted successfully: {FileName}", filename);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileName}", filename);
                return false;
            }
        }
    }
}

