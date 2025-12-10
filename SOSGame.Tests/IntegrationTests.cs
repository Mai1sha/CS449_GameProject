using SOSGame.Models;

namespace SOSGame.Tests
{
    public class IntegrationTests : IDisposable
    {
        private readonly string _testDirectory;

        public IntegrationTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), "SOSGameIntegrationTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        #region 8.1 Test complete recording workflow

        [Fact]
        public void CompleteRecordingWorkflow_SimpleGame_HumanVsHuman()
        {
            // Arrange - Start game with recording enabled
            var recorder = new GameRecorder();
            int boardSize = 3;
            var gameMode = GameMode.Simple;
            var bluePlayerType = PlayerType.Human;
            var redPlayerType = PlayerType.Human;

            recorder.StartRecording(boardSize, gameMode, bluePlayerType, redPlayerType);

            // Act - Play Simple game (Human vs Human)
            // Move 1: Blue places S at (0,0)
            recorder.RecordMove(0, 0, CellValue.S, Player.Blue);
            
            // Move 2: Red places O at (0,1)
            recorder.RecordMove(0, 1, CellValue.O, Player.Red);
            
            // Move 3: Blue places S at (0,2) - forms SOS, Blue wins
            recorder.RecordMove(0, 2, CellValue.S, Player.Blue);

            // Stop recording with winner
            recorder.StopRecording(Player.Blue, 1, 0);

            // Save to file
            string savedPath = recorder.SaveToFile();

            // Assert - Verify file created
            Assert.True(File.Exists(savedPath), "Recording file should be created");

            // Assert - Verify file contents
            string fileContent = File.ReadAllText(savedPath);
            
            // Check metadata section
            Assert.Contains("[METADATA]", fileContent);
            Assert.Contains("BoardSize=3", fileContent);
            Assert.Contains("GameMode=Simple", fileContent);
            Assert.Contains("BluePlayerType=Human", fileContent);
            Assert.Contains("RedPlayerType=Human", fileContent);
            Assert.Contains("Winner=Blue", fileContent);
            Assert.Contains("BlueScore=1", fileContent);
            Assert.Contains("RedScore=0", fileContent);

            // Check moves section
            Assert.Contains("[MOVES]", fileContent);
            Assert.Contains("1,0,0,S,Blue", fileContent);
            Assert.Contains("2,0,1,O,Red", fileContent);
            Assert.Contains("3,0,2,S,Blue", fileContent);
        }

        #endregion

        #region 8.2 Test complete replay workflow

        [Fact]
        public void CompleteReplayWorkflow_LoadAndNavigate_VerifyBoardState()
        {
            // Arrange - Create a recorded game file
            string filePath = Path.Combine(_testDirectory, "test_replay.sos");
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

            var replayer = new GameReplayer();

            // Act - Load recorded game
            var data = replayer.LoadFromFile(filePath);

            // Assert - Verify initial state
            Assert.NotNull(data);
            Assert.Equal(3, data.BoardSize);
            Assert.Equal(0, replayer.CurrentMoveIndex);
            Assert.True(replayer.HasNextMove);
            Assert.False(replayer.HasPreviousMove);

            // Act - Navigate through all moves and verify board state at each step
            
            // Move 1: Blue places S at (0,0)
            var move1 = replayer.GetNextMove();
            Assert.NotNull(move1);
            Assert.Equal(0, move1.Row);
            Assert.Equal(0, move1.Col);
            Assert.Equal(CellValue.S, move1.Value);
            Assert.Equal(Player.Blue, move1.Player);
            Assert.Equal(1, replayer.CurrentMoveIndex); // After getting move, index is incremented
            
            var movesAfterMove1 = replayer.GetMovesUpToCurrent();
            Assert.Single(movesAfterMove1);

            // Move 2: Red places O at (0,1)
            var move2 = replayer.GetNextMove();
            Assert.NotNull(move2);
            Assert.Equal(0, move2.Row);
            Assert.Equal(1, move2.Col); // Column 1, not 0
            Assert.Equal(CellValue.O, move2.Value);
            Assert.Equal(Player.Red, move2.Player);
            Assert.Equal(2, replayer.CurrentMoveIndex);
            
            var movesAfterMove2 = replayer.GetMovesUpToCurrent();
            Assert.Equal(2, movesAfterMove2.Count);

            // Move 3: Blue places S at (0,2)
            var move3 = replayer.GetNextMove();
            Assert.NotNull(move3);
            Assert.Equal(0, move3.Row);
            Assert.Equal(2, move3.Col); // Column 2, not 0
            Assert.Equal(CellValue.S, move3.Value);
            Assert.Equal(Player.Blue, move3.Player);
            Assert.Equal(3, replayer.CurrentMoveIndex);
            
            var movesAfterMove3 = replayer.GetMovesUpToCurrent();
            Assert.Equal(3, movesAfterMove3.Count);

            // Assert - Verify final outcome
            Assert.False(replayer.HasNextMove);
            Assert.Equal(Player.Blue, data.Winner);
            Assert.Equal(1, data.BlueScore);
            Assert.Equal(0, data.RedScore);
        }

        #endregion

        #region 8.3 Test all player configurations

        [Fact]
        public void RecordAndReplay_HumanVsHuman_WorksCorrectly()
        {
            // Arrange
            var recorder = new GameRecorder();
            recorder.StartRecording(3, GameMode.Simple, PlayerType.Human, PlayerType.Human);

            // Act - Record game
            recorder.RecordMove(0, 0, CellValue.S, Player.Blue);
            recorder.RecordMove(0, 1, CellValue.O, Player.Red);
            recorder.RecordMove(0, 2, CellValue.S, Player.Blue);
            recorder.StopRecording(Player.Blue, 1, 0);
            string savedPath = recorder.SaveToFile();

            // Act - Replay game
            var replayer = new GameReplayer();
            var data = replayer.LoadFromFile(savedPath);

            // Assert
            Assert.Equal(PlayerType.Human, data.BluePlayerType);
            Assert.Equal(PlayerType.Human, data.RedPlayerType);
            Assert.Equal(3, data.Moves.Count);
            Assert.Equal(Player.Blue, data.Winner);
        }

        [Fact]
        public void RecordAndReplay_HumanVsComputer_WorksCorrectly()
        {
            // Arrange
            var recorder = new GameRecorder();
            recorder.StartRecording(3, GameMode.Simple, PlayerType.Human, PlayerType.Computer);

            // Act - Record game
            recorder.RecordMove(0, 0, CellValue.S, Player.Blue);
            recorder.RecordMove(1, 1, CellValue.O, Player.Red); // Computer move
            recorder.RecordMove(0, 1, CellValue.O, Player.Blue);
            recorder.RecordMove(0, 2, CellValue.S, Player.Red); // Computer move - forms SOS
            recorder.StopRecording(Player.Red, 0, 1);
            string savedPath = recorder.SaveToFile();

            // Act - Replay game
            var replayer = new GameReplayer();
            var data = replayer.LoadFromFile(savedPath);

            // Assert
            Assert.Equal(PlayerType.Human, data.BluePlayerType);
            Assert.Equal(PlayerType.Computer, data.RedPlayerType);
            Assert.Equal(4, data.Moves.Count);
            Assert.Equal(Player.Red, data.Winner);
        }

        [Fact]
        public void RecordAndReplay_ComputerVsComputer_WorksCorrectly()
        {
            // Arrange
            var recorder = new GameRecorder();
            recorder.StartRecording(3, GameMode.General, PlayerType.Computer, PlayerType.Computer);

            // Act - Record a full game (Computer vs Computer)
            recorder.RecordMove(0, 0, CellValue.S, Player.Blue);
            recorder.RecordMove(0, 1, CellValue.O, Player.Red);
            recorder.RecordMove(0, 2, CellValue.S, Player.Blue); // Blue forms SOS
            recorder.RecordMove(1, 0, CellValue.S, Player.Red);
            recorder.RecordMove(1, 1, CellValue.O, Player.Blue);
            recorder.RecordMove(1, 2, CellValue.S, Player.Red); // Red forms SOS
            recorder.RecordMove(2, 0, CellValue.O, Player.Blue);
            recorder.RecordMove(2, 1, CellValue.S, Player.Red);
            recorder.RecordMove(2, 2, CellValue.O, Player.Blue);
            recorder.StopRecording(null, 1, 1); // Tie game
            string savedPath = recorder.SaveToFile();

            // Act - Replay game
            var replayer = new GameReplayer();
            var data = replayer.LoadFromFile(savedPath);

            // Assert
            Assert.Equal(PlayerType.Computer, data.BluePlayerType);
            Assert.Equal(PlayerType.Computer, data.RedPlayerType);
            Assert.Equal(9, data.Moves.Count);
            Assert.Null(data.Winner); // Tie game
            Assert.Equal(1, data.BlueScore);
            Assert.Equal(1, data.RedScore);
        }

        #endregion

        #region 8.4 Test error handling

        [Fact]
        public void ErrorHandling_CorruptedFile_ThrowsException()
        {
            // Arrange - Create corrupted file (invalid format)
            string filePath = Path.Combine(_testDirectory, "corrupted.sos");
            File.WriteAllText(filePath, "This is not a valid game file format");

            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));
        }

        [Fact]
        public void ErrorHandling_MissingFile_ThrowsException()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectory, "nonexistent.sos");
            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => replayer.LoadFromFile(filePath));
        }

        [Fact]
        public void ErrorHandling_InvalidMetadata_ThrowsException()
        {
            // Arrange - Create file with invalid metadata
            string filePath = Path.Combine(_testDirectory, "invalid_metadata.sos");
            string content = @"[METADATA]
BoardSize=invalid
GameMode=Simple

[MOVES]
1,0,0,S,Blue";
            File.WriteAllText(filePath, content);

            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));
        }

        [Fact]
        public void ErrorHandling_InvalidMoveData_ThrowsException()
        {
            // Arrange - Create file with invalid move data
            string filePath = Path.Combine(_testDirectory, "invalid_moves.sos");
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
1,invalid,0,S,Blue";
            File.WriteAllText(filePath, content);

            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));
        }

        [Fact]
        public void ErrorHandling_MissingMetadataSection_ThrowsException()
        {
            // Arrange - Create file without metadata section
            string filePath = Path.Combine(_testDirectory, "no_metadata.sos");
            string content = @"[MOVES]
1,0,0,S,Blue";
            File.WriteAllText(filePath, content);

            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));
        }

        [Fact]
        public void ErrorHandling_MissingMovesSection_ThrowsException()
        {
            // Arrange - Create file without moves section
            string filePath = Path.Combine(_testDirectory, "no_moves.sos");
            string content = @"[METADATA]
BoardSize=3
GameMode=Simple";
            File.WriteAllText(filePath, content);

            var replayer = new GameReplayer();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => replayer.LoadFromFile(filePath));
        }

        #endregion

        #region 8.5 Test UI integration (functional verification)

        [Fact]
        public void UIIntegration_RecordingCheckbox_EnablesRecording()
        {
            // This test verifies the recording functionality can be enabled
            // In a real UI test, this would verify the checkbox state
            
            // Arrange
            bool recordingEnabled = true; // Simulates checkbox checked
            var recorder = new GameRecorder();

            // Act
            if (recordingEnabled)
            {
                recorder.StartRecording(3, GameMode.Simple, PlayerType.Human, PlayerType.Human);
            }

            // Assert
            Assert.True(recorder.IsRecording);
        }

        [Fact]
        public void UIIntegration_ReplayButton_LoadsGameFile()
        {
            // This test verifies the replay functionality can load a file
            // In a real UI test, this would verify the button click and file dialog
            
            // Arrange - Create a test file
            string filePath = Path.Combine(_testDirectory, "ui_test.sos");
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

            var replayer = new GameReplayer();

            // Act - Simulate replay button click and file selection
            var data = replayer.LoadFromFile(filePath);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(3, data.BoardSize);
        }

        [Fact]
        public void UIIntegration_NavigationButtons_UpdateGameState()
        {
            // This test verifies navigation buttons work correctly
            // In a real UI test, this would verify button clicks and UI updates
            
            // Arrange
            string filePath = Path.Combine(_testDirectory, "nav_test.sos");
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

            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Act - Simulate Next button clicks
            Assert.True(replayer.HasNextMove);
            var move1 = replayer.GetNextMove();
            Assert.NotNull(move1);

            Assert.True(replayer.HasNextMove);
            var move2 = replayer.GetNextMove();
            Assert.NotNull(move2);

            // Act - Simulate Previous button click
            Assert.True(replayer.HasPreviousMove);
            var prevMove = replayer.GetPreviousMove();
            Assert.NotNull(prevMove);
            Assert.Equal(move2.Row, prevMove.Row);
            Assert.Equal(move2.Col, prevMove.Col);

            // Act - Simulate Reset button click
            replayer.Reset();
            Assert.Equal(0, replayer.CurrentMoveIndex);
            Assert.False(replayer.HasPreviousMove);
            Assert.True(replayer.HasNextMove);
        }

        [Fact]
        public void UIIntegration_AllControlsVisible_Functional()
        {
            // This test verifies all required controls are functional
            // In a real UI test, this would verify control visibility and state
            
            // Arrange
            string filePath = Path.Combine(_testDirectory, "controls_test.sos");
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

            var replayer = new GameReplayer();
            replayer.LoadFromFile(filePath);

            // Assert - Verify move counter functionality
            Assert.Equal(0, replayer.CurrentMoveIndex);
            Assert.Equal(3, replayer.TotalMoves);

            // Assert - Verify navigation button states
            Assert.False(replayer.HasPreviousMove); // Previous should be disabled at start
            Assert.True(replayer.HasNextMove); // Next should be enabled

            // Move to end
            replayer.GetNextMove();
            replayer.GetNextMove();
            replayer.GetNextMove();

            Assert.True(replayer.HasPreviousMove); // Previous should be enabled at end
            Assert.False(replayer.HasNextMove); // Next should be disabled at end
        }

        #endregion
    }
}
