namespace SOSGame.Models
{
    public class GameState
    {
        private readonly Board _board;
        private readonly GameMode _gameMode;
        private readonly GameLogic _gameLogic;
        private Player _currentPlayer;

        public Board Board => _board;
        public GameMode Mode => _gameMode;
        public Player CurrentPlayer => _currentPlayer;
        public GameLogic GameLogic => _gameLogic;
        public bool IsGameOver => _gameLogic.IsGameOver;
        public Player? Winner => _gameLogic.Winner;
        public int BlueScore => _gameLogic.BlueScore;
        public int RedScore => _gameLogic.RedScore;

        public GameState(int boardSize, GameMode gameMode = GameMode.Simple)
        {
            _board = new Board(boardSize);
            _gameMode = gameMode;
            _currentPlayer = Player.Blue;

            _gameLogic = gameMode == GameMode.Simple 
                ? new SimpleGameLogic(_board) 
                : new GeneralGameLogic(_board);
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
