namespace SOSGame.Models
{
    public class GameState
    {
        private readonly Board _board;
        private readonly GameMode _gameMode;
        private readonly GameLogic _gameLogic;
        private Player _currentPlayer;
        private readonly PlayerController _bluePlayerController;
        private readonly PlayerController _redPlayerController;

        public Board Board => _board;
        public GameMode Mode => _gameMode;
        public Player CurrentPlayer => _currentPlayer;
        public GameLogic GameLogic => _gameLogic;
        public bool IsGameOver => _gameLogic.IsGameOver;
        public Player? Winner => _gameLogic.Winner;
        public int BlueScore => _gameLogic.BlueScore;
        public int RedScore => _gameLogic.RedScore;
        public PlayerController BluePlayerController => _bluePlayerController;
        public PlayerController RedPlayerController => _redPlayerController;
        public PlayerController CurrentPlayerController => 
            _currentPlayer == Player.Blue ? _bluePlayerController : _redPlayerController;

        public GameState(int boardSize, GameMode gameMode = GameMode.Simple, 
            PlayerType bluePlayerType = PlayerType.Human, PlayerType redPlayerType = PlayerType.Human)
        {
            _board = new Board(boardSize);
            _gameMode = gameMode;
            _currentPlayer = Player.Blue;

            _gameLogic = gameMode == GameMode.Simple 
                ? new SimpleGameLogic(_board) 
                : new GeneralGameLogic(_board);

            // Initialize player controllers based on player type
            _bluePlayerController = CreatePlayerController(Player.Blue, bluePlayerType);
            _redPlayerController = CreatePlayerController(Player.Red, redPlayerType);
        }

        private PlayerController CreatePlayerController(Player player, PlayerType playerType)
        {
            return playerType == PlayerType.Computer
                ? new ComputerPlayerController(player)
                : new HumanPlayerController(player);
        }

        public bool MakeMove(int row, int col, CellValue cellValue)
        {
            if (_gameLogic.IsGameOver)
                return false;

            bool moveSuccessful = _board.PlaceMove(row, col, cellValue);

            if (moveSuccessful)
            {
                List<SOSSequence> sosSequences = _gameLogic.CheckForSOS(row, col, _currentPlayer);
                
                _gameLogic.UpdateGameState(sosSequences, _currentPlayer);
                
                if (_gameLogic.ShouldSwitchPlayer(sosSequences))
                {
                    SwitchPlayer();
                }
            }

            return moveSuccessful;
        }

        /// <summary>
        /// Gets the computer move for the current player if they are a computer.
        /// Returns null if current player is human or if game is over.
        /// </summary>
        public Move? GetComputerMove()
        {
            if (_gameLogic.IsGameOver)
                return null;

            if (!CurrentPlayerController.IsComputer())
                return null;

            return CurrentPlayerController.GetMove(_board, _gameLogic, _gameMode);
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == Player.Blue ? Player.Red : Player.Blue;
        }

        public void Reset()
        {
            _board.Reset();
            _gameLogic.Reset();
            _currentPlayer = Player.Blue;
        }
    }
}
