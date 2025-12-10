namespace SOSGame.Models
{
    public class GameRecorder
    {
        private GameRecordingData? _recordingData;
        private bool _isRecording;
        private int _moveCounter;

        public bool IsRecording => _isRecording;

        public GameRecorder()
        {
            _recordingData = null;
            _isRecording = false;
            _moveCounter = 0;
        }

        /// <summary>
        /// Starts recording a game session with the specified metadata.
        /// </summary>
        public void StartRecording(int boardSize, GameMode gameMode, 
            PlayerType bluePlayerType, PlayerType redPlayerType)
        {
            _recordingData = new GameRecordingData(
                boardSize,
                gameMode,
                bluePlayerType,
                redPlayerType,
                DateTime.Now
            );
            _moveCounter = 0;
            _isRecording = true;
        }

        /// <summary>
        /// Records a move made during the game.
        /// </summary>
        public void RecordMove(int row, int col, CellValue value, Player player)
        {
            if (!_isRecording || _recordingData == null)
            {
                throw new InvalidOperationException("Recording has not been started");
            }

            _moveCounter++;
            var recordedMove = new RecordedMove(row, col, value, player, _moveCounter);
            _recordingData.Moves.Add(recordedMove);
        }

        /// <summary>
        /// Stops recording and captures the final game state.
        /// </summary>
        public void StopRecording(Player? winner, int blueScore, int redScore)
        {
            if (!_isRecording || _recordingData == null)
            {
                throw new InvalidOperationException("Recording has not been started");
            }

            _recordingData.Winner = winner;
            _recordingData.BlueScore = blueScore;
            _recordingData.RedScore = redScore;
            _isRecording = false;
        }

        /// <summary>
        /// Saves the recorded game to a file in the Recordings directory.
        /// Returns the full path to the saved file.
        /// </summary>
        public string SaveToFile()
        {
            if (_recordingData == null)
            {
                throw new InvalidOperationException("No recording data to save");
            }

            RecordingsDirectoryManager.EnsureRecordingsDirectoryExists();

            string filename = RecordingsDirectoryManager.GenerateUniqueFilename(
                _recordingData.GameMode, 
                _recordingData.Timestamp);
            
            string fullPath = RecordingsDirectoryManager.ConstructFilePath(filename);

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("[METADATA]");
                writer.WriteLine($"BoardSize={_recordingData.BoardSize}");
                writer.WriteLine($"GameMode={_recordingData.GameMode}");
                writer.WriteLine($"BluePlayerType={_recordingData.BluePlayerType}");
                writer.WriteLine($"RedPlayerType={_recordingData.RedPlayerType}");
                writer.WriteLine($"Timestamp={_recordingData.Timestamp:O}");
                writer.WriteLine($"Winner={(_recordingData.Winner?.ToString() ?? "null")}");
                writer.WriteLine($"BlueScore={_recordingData.BlueScore}");
                writer.WriteLine($"RedScore={_recordingData.RedScore}");
                writer.WriteLine();

                writer.WriteLine("[MOVES]");
                foreach (var move in _recordingData.Moves)
                {
                    writer.WriteLine($"{move.MoveNumber},{move.Row},{move.Col},{move.Value},{move.Player}");
                }
            }

            return fullPath;
        }
    }
}
