using SOSGame.Models;
using Xunit;

namespace SOSGame.Tests
{
    public class RecordingsDirectoryManagerTests
    {
        [Fact]
        public void GetRecordingsDirectoryPath_ReturnsPathWithRecordingsFolder()
        {
            // Act
            string recordingsPath = RecordingsDirectoryManager.GetRecordingsDirectoryPath();

            // Assert
            Assert.Contains("Recordings", recordingsPath);
        }

        [Fact]
        public void EnsureRecordingsDirectoryExists_CreatesDirectory()
        {
            // Act
            bool result = RecordingsDirectoryManager.EnsureRecordingsDirectoryExists();
            string recordingsPath = RecordingsDirectoryManager.GetRecordingsDirectoryPath();

            // Assert
            Assert.True(result);
            Assert.True(Directory.Exists(recordingsPath));
        }

        [Fact]
        public void ConstructFilePath_WithValidFilename_ReturnsFullPath()
        {
            // Arrange
            string filename = "test_game.sos";

            // Act
            string fullPath = RecordingsDirectoryManager.ConstructFilePath(filename);

            // Assert
            Assert.Contains("Recordings", fullPath);
            Assert.EndsWith(".sos", fullPath);
        }

        [Fact]
        public void ConstructFilePath_WithoutExtension_AddsExtension()
        {
            // Arrange
            string filename = "test_game";

            // Act
            string fullPath = RecordingsDirectoryManager.ConstructFilePath(filename);

            // Assert
            Assert.EndsWith(".sos", fullPath);
        }

        [Fact]
        public void ConstructFilePath_WithNullFilename_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                RecordingsDirectoryManager.ConstructFilePath(null!));
        }

        [Fact]
        public void ConstructFilePath_WithEmptyFilename_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                RecordingsDirectoryManager.ConstructFilePath(""));
        }

        [Fact]
        public void GenerateUniqueFilename_ReturnsValidFilename()
        {
            // Arrange
            GameMode gameMode = GameMode.Simple;
            DateTime timestamp = DateTime.Now;

            // Act
            string filename = RecordingsDirectoryManager.GenerateUniqueFilename(gameMode, timestamp);

            // Assert
            Assert.Contains("SOSGame", filename);
            Assert.Contains("Simple", filename);
            Assert.EndsWith(".sos", filename);
        }

        [Fact]
        public void GenerateUniqueFilename_WithExistingFile_GeneratesUniqueFilename()
        {
            // Arrange
            RecordingsDirectoryManager.EnsureRecordingsDirectoryExists();
            GameMode gameMode = GameMode.General;
            DateTime timestamp = DateTime.Now;
            
            // Create first file
            string filename1 = RecordingsDirectoryManager.GenerateUniqueFilename(gameMode, timestamp);
            string fullPath1 = RecordingsDirectoryManager.ConstructFilePath(filename1);
            File.WriteAllText(fullPath1, "test");

            try
            {
                // Act - Generate another filename with same timestamp
                string filename2 = RecordingsDirectoryManager.GenerateUniqueFilename(gameMode, timestamp);

                // Assert
                Assert.NotEqual(filename1, filename2);
            }
            finally
            {
                // Cleanup
                if (File.Exists(fullPath1))
                {
                    File.Delete(fullPath1);
                }
            }
        }
    }
}
