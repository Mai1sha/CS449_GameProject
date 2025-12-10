using System;
using System.IO;

namespace SOSGame.Models
{
    /// <summary>
    /// Utility class for managing the Recordings directory and file paths.
    /// Handles directory creation, path validation, and error handling.
    /// </summary>
    public static class RecordingsDirectoryManager
    {
        private const string RecordingsDirectoryName = "Recordings";

        public static string GetRecordingsDirectoryPath()
        {
            string appDirectory = GetApplicationDirectory();
            return Path.Combine(appDirectory, RecordingsDirectoryName);
        }

        private static string GetApplicationDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static bool EnsureRecordingsDirectoryExists()
        {
            string recordingsPath = GetRecordingsDirectoryPath();

            try
            {
                if (!Directory.Exists(recordingsPath))
                {
                    Directory.CreateDirectory(recordingsPath);
                }
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException(
                    $"Permission denied: Cannot create Recordings directory at '{recordingsPath}'. " +
                    $"Please check folder permissions.", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(
                    $"I/O error: Cannot create Recordings directory at '{recordingsPath}'. " +
                    $"Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unexpected error creating Recordings directory at '{recordingsPath}'. " +
                    $"Error: {ex.Message}", ex);
            }
        }

        public static string ConstructFilePath(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be null or empty.", nameof(filename));
            }

            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("Filename contains invalid characters.", nameof(filename));
            }

            if (!filename.EndsWith(".sos", StringComparison.OrdinalIgnoreCase))
            {
                filename += ".sos";
            }

            string recordingsPath = GetRecordingsDirectoryPath();
            return Path.Combine(recordingsPath, filename);
        }

        public static string GenerateUniqueFilename(GameMode gameMode, DateTime timestamp)
        {
            string timestampStr = timestamp.ToString("yyyyMMdd_HHmmss");
            string baseFilename = $"SOSGame_{gameMode}_{timestampStr}";
            string filename = $"{baseFilename}.sos";
            string fullPath = ConstructFilePath(filename);

            int counter = 1;
            while (File.Exists(fullPath))
            {
                filename = $"{baseFilename}_{counter}.sos";
                fullPath = ConstructFilePath(filename);
                counter++;
            }

            return filename;
        }
    }
}
