using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eraasoftTask13.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Saves an uploaded file to wwwroot/uploads/{subfolder} and returns the relative URL path.
        /// Returns null if the file is null or invalid.
        /// </summary>
        public async Task<string?> SaveFileAsync(IFormFile? file, string subfolder)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > MaxFileSizeBytes)
                throw new InvalidOperationException($"File size exceeds the maximum allowed size of 10 MB.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!Array.Exists(AllowedExtensions, e => e == ext))
                throw new InvalidOperationException($"File type '{ext}' is not allowed. Only JPG, JPEG, PNG, and WEBP are accepted.");

            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(uploadFolder);

            var uniqueName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{subfolder}/{uniqueName}";
        }

        /// <summary>
        /// Deletes a file given its relative URL path (e.g., /uploads/actors/abc.jpg).
        /// Does nothing if the path is null/empty or the file does not exist.
        /// </summary>
        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            // Ignore external URLs (seeded with https:// etc.)
            if (relativePath.StartsWith("http://") || relativePath.StartsWith("https://"))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// Validates a file without saving it. Returns an error message or null if valid.
        /// </summary>
        public string? Validate(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null; // Optional files are OK

            if (file.Length > MaxFileSizeBytes)
                return "File size must not exceed 10 MB.";

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!Array.Exists(AllowedExtensions, e => e == ext))
                return $"Only JPG, JPEG, PNG, and WEBP files are accepted. You uploaded: {ext}";

            return null;
        }
    }
}
