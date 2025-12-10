using SOSGame.Models;

namespace SOSGame.Tests
{
    public class GameReplayerTests
    {
        private string CreateTestRecordingFile(string filename)
        {
            string directory = Path.Combine(Path.GetTempPath(), "SOSGameTests");
            Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, filename);

            string content = @"[METADATA]
BoardSize=3
GameMode=Simple
BluePlayerType=Human
RedPlayerType=Human
Timestamp=2024-12-06T14:30:00
Winner=Blue
BlueScore=1
RedScore=0

[MOVES]
1,0,0,S,Blue
2,0,1,O,Red
3,0,2,S,Blue";

            File.WriteAllText(filePath, content);
            return filePath;
        }

        [Fact]
        public void LoadFromFile_LoadsValidFile_Successfully()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_valid.sos");
            var replayer = new GameReplayer();

            // Act
            var data = replayer.LoadFromFile(filePath);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(3, data.BoardSize);
            Assert.Equal(GameMode.Simple, data.GameMode);
            Assert.Equal(PlayerType.Human, data.BluePlayerType);
            Assert.Equal(PlayerType.Human, data.RedPlayerType);
            Assert.Equal(Player.Blue, data.Winner);
            Assert.Equal(1, data.BlueScore);
            Assert.Equal(0, data.RedScore);
            Assert.Equal(3, data.Moves.Count);

            // Cleanup
            File.Delete(filePath);
        }


        [Fact]
        public void LoadFromFile_ThrowsException_WhenFileNotFound()
        {
            // Arrange
            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                replayer.LoadFromFile("nonexistent.sos"));
        }

        [Fact]
        public void LoadFromFile_ThrowsException_WhenMetadataSectionMissing()
        {
            // Arrange
            string filePath = Path.Combine(Path.GetTempPath(), "test_no_metadata.sos");
            File.WriteAllText(filePath, "[MOVES]\n1,0,0,S,Blue");
            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void LoadFromFile_ThrowsException_WhenMovesSectionMissing()
        {
            // Arrange
            string filePath = Path.Combine(Path.GetTempPath(), "test_no_moves.sos");
            File.WriteAllText(filePath, "[METADATA]\nBoardSize=3");
            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void GetNextMove_ReturnsNextMove_WhenAvailable()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_next.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Act
            var move = replayer.GetNextMove();

            // Assert
            Assert.NotNull(move);
            Assert.Equal(0, move.Row);
            Assert.Equal(0, move.Col);
            Assert.Equal(CellValue.S, move.Value);
            Assert.Equal(Player.Blue, move.Player);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void GetNextMove_ReturnsNull_WhenAtEnd()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_end.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Advance to end
            replayer.GetNextMove();
            replayer.GetNextMove();
            replayer.GetNextMove();

            // Act
            var move = replayer.GetNextMove();

            // Assert
            Assert.Null(move);

            // Cleanup
            File.Delete(filePath);
        }


        [Fact]
        public void GetPreviousMove_ReturnsPreviousMove_WhenAvailable()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_prev.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Advance then go back
            replayer.GetNextMove(); // Move to index 1 (move 1)
            replayer.GetNextMove(); // Move to index 2 (move 2)

            // Act
            var move = replayer.GetPreviousMove(); // Go back to index 1 (move 2)

            // Assert
            Assert.NotNull(move);
            Assert.Equal(0, move.Row);
            Assert.Equal(1, move.Col); // Second move is at column 1
            Assert.Equal(CellValue.O, move.Value);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void GetPreviousMove_ReturnsNull_WhenAtBeginning()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_begin.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Act
            var move = replayer.GetPreviousMove();

            // Assert
            Assert.Null(move);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Reset_ResetsToBeginning()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_reset.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Advance
            replayer.GetNextMove();
            replayer.GetNextMove();

            // Act
            replayer.Reset();

            // Assert
            Assert.Equal(0, replayer.CurrentMoveIndex);
            Assert.True(replayer.HasNextMove);
            Assert.False(replayer.HasPreviousMove);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void GetMovesUpToCurrent_ReturnsCorrectMoves()
        {
            // Arrange
            string filePath = CreateTestRecordingFile("test_moves_up_to.sos");
            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Advance to move 2
            replayer.GetNextMove();
            replayer.GetNextMove();

            // Act
            var moves = replayer.GetMovesUpToCurrent();

            // Assert
            Assert.Equal(2, moves.Count);
            Assert.Equal(0, moves[0].Row);
            Assert.Equal(0, moves[0].Col);
            Assert.Equal(0, moves[1].Row);
            Assert.Equal(1, moves[1].Col);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
