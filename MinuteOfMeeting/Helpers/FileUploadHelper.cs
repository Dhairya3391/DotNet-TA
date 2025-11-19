using Microsoft.AspNetCore.Http;

namespace MinuteOfMeeting.Helpers
{
    /// <summary>
    /// File Upload Helper Class
    /// Handles file upload operations with validation and security
    /// </summary>
    public static class FileUploadHelper
    {
        // Allowed file extensions for meeting documents
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx" };

        // Maximum file size (5MB)
        private const long MaxFileSize = 5 * 1024 * 1024;

        /// <summary>
        /// Upload file to specified folder
        /// </summary>
        /// <param name="file">IFormFile to upload</param>
        /// <param name="folder">Target folder within wwwroot/uploads</param>
        /// <returns>Relative file path if successful, null otherwise</returns>
        public static async Task<string> UploadFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                // Validate file type
                string extension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtensions.Contains(extension))
                {
                    throw new Exception($"Invalid file type. Allowed types: {string.Join(", ", AllowedExtensions)}");
                }

                // Validate file size
                if (file.Length > MaxFileSize)
                {
                    throw new Exception($"File size cannot exceed {MaxFileSize / (1024 * 1024)}MB");
                }

                // Generate unique filename
                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);

                // Ensure directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for database storage
                return $"/uploads/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete file from server
        /// </summary>
        /// <param name="filePath">Relative file path</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        public static bool DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="filePath">Relative file path</param>
        /// <returns>True if file exists, false otherwise</returns>
        public static bool FileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                return File.Exists(fullPath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get file information
        /// </summary>
        /// <param name="filePath">Relative file path</param>
        /// <returns>File info object or null if file doesn't exist</returns>
        public static FileInfo GetFileInfo(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                return new FileInfo(fullPath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get file size in human readable format
        /// </summary>
        /// <param name="bytes">File size in bytes</param>
        /// <returns>Formatted file size string</returns>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Get file icon based on extension
        /// </summary>
        /// <param name="filePath">File path or file name</param>
        /// <returns>Bootstrap icon class</returns>
        public static string GetFileIcon(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return "bi-file-earmark";

            string extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
                ".pdf" => "bi-file-earmark-pdf",
                ".doc" or ".docx" => "bi-file-earmark-word",
                ".xls" or ".xlsx" => "bi-file-earmark-excel",
                ".ppt" or ".pptx" => "bi-file-earmark-slides",
                ".txt" => "bi-file-earmark-text",
                ".jpg" or ".jpeg" or ".png" or ".gif" => "bi-file-earmark-image",
                _ => "bi-file-earmark"
            };
        }

        /// <summary>
        /// Validate uploaded file
        /// </summary>
        /// <param name="file">IFormFile to validate</param>
        /// <returns>Validation result with error message if any</returns>
        public static (bool IsValid, string ErrorMessage) ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "Please select a file");
            }

            // Check file extension
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
            {
                return (false, $"Invalid file type. Allowed types: {string.Join(", ", AllowedExtensions)}");
            }

            // Check file size
            if (file.Length > MaxFileSize)
            {
                return (false, $"File size cannot exceed {MaxFileSize / (1024 * 1024)}MB");
            }

            // Check file name for invalid characters
            string fileName = Path.GetFileName(file.FileName);
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return (false, "File name contains invalid characters");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Create backup of existing file before replacement
        /// </summary>
        /// <param name="filePath">File path to backup</param>
        /// <returns>Backup file path or null if failed</returns>
        public static string CreateBackup(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !FileExists(filePath))
                return null;

            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                string backupPath = $"{fullPath}.backup.{DateTime.Now:yyyyMMddHHmmss}";

                File.Copy(fullPath, backupPath);

                return backupPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Clean up old backup files
        /// </summary>
        /// <param name="folder">Folder to clean</param>
        /// <param name="daysToKeep">Number of days to keep backups</param>
        public static void CleanupOldBackups(string folder, int daysToKeep = 7)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);

                if (!Directory.Exists(uploadsFolder))
                    return;

                var backupFiles = Directory.GetFiles(uploadsFolder, "*.backup.*")
                    .Where(f => File.GetCreationTime(f) < DateTime.Now.AddDays(-daysToKeep));

                foreach (string backupFile in backupFiles)
                {
                    try
                    {
                        File.Delete(backupFile);
                    }
                    catch
                    {
                        // Continue with other files if one fails
                    }
                }
            }
            catch
            {
                // Log error if needed
            }
        }

        /// <summary>
        /// Get upload statistics
        /// </summary>
        /// <param name="folder">Folder to analyze</param>
        /// <returns>Upload statistics</returns>
        public static (int FileCount, long TotalSize) GetUploadStats(string folder)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);

                if (!Directory.Exists(uploadsFolder))
                    return (0, 0);

                var files = Directory.GetFiles(uploadsFolder);
                long totalSize = files.Sum(f => new FileInfo(f).Length);

                return (files.Length, totalSize);
            }
            catch
            {
                return (0, 0);
            }
        }
    }
}