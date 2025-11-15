using System.IO;
using System.Threading.Tasks;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IFileUploadService
    {
        Task<FileUploadResult> UploadImageAsync(Stream fileStream, string fileName, long fileSize);
        Task<FileUploadResult> UploadDocumentAsync(Stream fileStream, string fileName, long fileSize);
        Task<bool> DeleteFileAsync(string filename);
    }

    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

