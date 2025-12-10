namespace SOSGame.Models
{
    public class GameReplayer
    {
        private GameRecordingData? _recordingData;
        private int _currentMoveIndex;

        public GameRecordingData? RecordingData => _recordingData;
        public int CurrentMoveIndex => _currentMoveIndex;
        public int TotalMoves => _recordingData?.Moves.Count ?? 0;
        public bool HasNextMove => _currentMoveIndex < TotalMoves;
        public bool HasPreviousMove => _currentMoveIndex > 0;

        public GameReplayer()
        {
            _recordingData = null;
            _currentMoveIndex = 0;
        }

        /// <summary>
        /// Loads a game recording from a file and validates the data.
        /// Returns the loaded GameRecordingData.
        /// Throws exceptions for invalid files.
        /// </summary>
        public GameRecordingData LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Game recording file not found: {filePath}");
            }

            string[] lines = File.ReadAllLines(filePath);

            int metadataStart = -1;
            int movesStart = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line == "[METADATA]")
                    metadataStart = i;
                else if (line == "[MOVES]")
                    movesStart = i;
            }

            if (metadataStart == -1)
            {
                throw new InvalidDataException("Missing [METADATA] section in game recording file");
            }
            if (movesStart == -1)
            {
                throw new InvalidDataException("Missing [MOVES] section in game recording file");
            }

            Dictionary<string, string> metadata = new Dictionary<string, string>();
            for (int i = metadataStart + 1; i < movesStart; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    metadata[parts[0].Trim()] = parts[1].Trim();
                }
            }

            if (!metadata.ContainsKey("BoardSize"))
                throw new InvalidDataException("Missing required field: BoardSize");
            if (!metadata.ContainsKey("GameMode"))
                throw new InvalidDataException("Missing required field: GameMode");
            if (!metadata.ContainsKey("BluePlayerType"))
                throw new InvalidDataException("Missing required field: BluePlayerType");
            if (!metadata.ContainsKey("RedPlayerType"))
                throw new InvalidDataException("Missing required field: RedPlayerType");
            if (!metadata.ContainsKey("Timestamp"))
                throw new InvalidDataException("Missing required field: Timestamp");
            if (!metadata.ContainsKey("Winner"))
                throw new InvalidDataException("Missing required field: Winner");
            if (!metadata.ContainsKey("BlueScore"))
                throw new InvalidDataException("Missing required field: BlueScore");
            if (!metadata.ContainsKey("RedScore"))
                throw new InvalidDataException("Missing required field: RedScore");

            // Parse metadata values
            if (!int.TryParse(metadata["BoardSize"], out int boardSize) || boardSize < 3)
                throw new InvalidDataException($"Invalid BoardSize: {metadata["BoardSize"]}");

            if (!Enum.TryParse<GameMode>(metadata["GameMode"], out GameMode gameMode))
                throw new InvalidDataException($"Invalid GameMode: {metadata["GameMode"]}");

            if (!Enum.TryParse<PlayerType>(metadata["BluePlayerType"], out PlayerType bluePlayerType))
                throw new InvalidDataException($"Invalid BluePlayerType: {metadata["BluePlayerType"]}");

            if (!Enum.TryParse<PlayerType>(metadata["RedPlayerType"], out PlayerType redPlayerType))
                throw new InvalidDataException($"Invalid RedPlayerType: {metadata["RedPlayerType"]}");

            if (!DateTime.TryParse(metadata["Timestamp"], out DateTime timestamp))
                throw new InvalidDataException($"Invalid Timestamp: {metadata["Timestamp"]}");

            Player? winner = null;
            if (metadata["Winner"] != "null")
            {
                if (!Enum.TryParse<Player>(metadata["Winner"], out Player winnerValue))
                    throw new InvalidDataException($"Invalid Winner: {metadata["Winner"]}");
                winner = winnerValue;
            }

            if (!int.TryParse(metadata["BlueScore"], out int blueScore) || blueScore < 0)
                throw new InvalidDataException($"Invalid BlueScore: {metadata["BlueScore"]}");

            if (!int.TryParse(metadata["RedScore"], out int redScore) || redScore < 0)
                throw new InvalidDataException($"Invalid RedScore: {metadata["RedScore"]}");

            List<RecordedMove> moves = new List<RecordedMove>();
            for (int i = movesStart + 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');
                if (parts.Length != 5)
                    throw new InvalidDataException($"Invalid move format at line {i + 1}: {line}");

                if (!int.TryParse(parts[0], out int moveNumber) || moveNumber < 1)
                    throw new InvalidDataException($"Invalid move number at line {i + 1}: {parts[0]}");

                if (!int.TryParse(parts[1], out int row) || row < 0 || row >= boardSize)
                    throw new InvalidDataException($"Invalid row coordinate at line {i + 1}: {parts[1]}");

                if (!int.TryParse(parts[2], out int col) || col < 0 || col >= boardSize)
                    throw new InvalidDataException($"Invalid column coordinate at line {i + 1}: {parts[2]}");

                if (!Enum.TryParse<CellValue>(parts[3], out CellValue value) || value == CellValue.Empty)
                    throw new InvalidDataException($"Invalid cell value at line {i + 1}: {parts[3]}");

                if (!Enum.TryParse<Player>(parts[4], out Player player))
                    throw new InvalidDataException($"Invalid player at line {i + 1}: {parts[4]}");

                moves.Add(new RecordedMove(row, col, value, player, moveNumber));
            }

            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].MoveNumber != i + 1)
                    throw new InvalidDataException($"Move sequence is not continuous. Expected move {i + 1}, found {moves[i].MoveNumber}");
            }

            _recordingData = new GameRecordingData(
                boardSize,
                gameMode,
                bluePlayerType,
                redPlayerType,
                timestamp,
                moves,
                winner,
                blueScore,
                redScore
            );

            _currentMoveIndex = 0;
            return _recordingData;
        }

        /// <summary>
        /// Advances to the next move in the replay.
        /// Returns the next move or null if at the end.
        /// </summary>
        public RecordedMove? GetNextMove()
        {
            if (_recordingData == null)
            {
                throw new InvalidOperationException("No game recording loaded");
            }

            if (!HasNextMove)
            {
                return null;
            }

            RecordedMove move = _recordingData.Moves[_currentMoveIndex];
            _currentMoveIndex++;
            return move;
        }

        /// <summary>
        /// Goes back to the previous move in the replay.
        /// Returns the previous move or null if at the beginning.
        /// </summary>
        public RecordedMove? GetPreviousMove()
        {
            if (_recordingData == null)
            {
                throw new InvalidOperationException("No game recording loaded");
            }

            if (!HasPreviousMove)
            {
                return null;
            }

            _currentMoveIndex--;
            return _recordingData.Moves[_currentMoveIndex];
        }

        /// <summary>
        /// Resets the replay to the beginning.
        /// </summary>
        public void Reset()
        {
            _currentMoveIndex = 0;
        }

        /// <summary>
        /// Gets all moves up to the current position.
        /// Used to reconstruct the board state at the current point in the replay.
        /// </summary>
        public List<RecordedMove> GetMovesUpToCurrent()
        {
            if (_recordingData == null)
            {
                throw new InvalidOperationException("No game recording loaded");
            }

            return _recordingData.Moves.Take(_currentMoveIndex).ToList();
        }
    }
}
