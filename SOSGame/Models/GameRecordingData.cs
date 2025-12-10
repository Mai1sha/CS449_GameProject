namespace SOSGame.Models
{
    public class GameRecordingData
    {
        public int BoardSize { get; set; }
        public GameMode GameMode { get; set; }
        public PlayerType BluePlayerType { get; set; }
        public PlayerType RedPlayerType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<RecordedMove> Moves { get; set; }
        public Player? Winner { get; set; }
        public int BlueScore { get; set; }
        public int RedScore { get; set; }

        public GameRecordingData(
            int boardSize,
            GameMode gameMode,
            PlayerType bluePlayerType,
            PlayerType redPlayerType,
            DateTime timestamp)
        {
            // Validation
            if (boardSize < 3)
                throw new ArgumentException("Board size must be at least 3", nameof(boardSize));

            BoardSize = boardSize;
            GameMode = gameMode;
            BluePlayerType = bluePlayerType;
            RedPlayerType = redPlayerType;
            Timestamp = timestamp;
            Moves = new List<RecordedMove>();
            Winner = null;
            BlueScore = 0;
            RedScore = 0;
        }

        public GameRecordingData(
            int boardSize,
            GameMode gameMode,
            PlayerType bluePlayerType,
            PlayerType redPlayerType,
            DateTime timestamp,
            List<RecordedMove> moves,
            Player? winner,
            int blueScore,
            int redScore)
        {
            // Validation
            if (boardSize < 3)
                throw new ArgumentException("Board size must be at least 3", nameof(boardSize));
            if (blueScore < 0)
                throw new ArgumentException("Blue score must be non-negative", nameof(blueScore));
            if (redScore < 0)
                throw new ArgumentException("Red score must be non-negative", nameof(redScore));

            BoardSize = boardSize;
            GameMode = gameMode;
            BluePlayerType = bluePlayerType;
            RedPlayerType = redPlayerType;
            Timestamp = timestamp;
            Moves = moves ?? new List<RecordedMove>();
            Winner = winner;
            BlueScore = blueScore;
            RedScore = redScore;
        }
    }
}
