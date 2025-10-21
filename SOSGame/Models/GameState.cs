namespace SOSGame.Models
{
    public class GameState
    {
        private readonly Board _board;
        private readonly GameMode _gameMode;
        private Player _currentPlayer;

        public Board Board => _board;
        public GameMode Mode => _gameMode;
        public Player CurrentPlayer => _currentPlayer;

        public GameState(int boardSize, GameMode gameMode = GameMode.Simple)
        {
            _board = new Board(boardSize);
            _gameMode = gameMode;
            _currentPlayer = Player.Blue;
        }

        public bool MakeMove(int row, int col, CellValue cellValue)
        {
            bool moveSuccessful = _board.PlaceMove(row, col, cellValue);

            if (moveSuccessful)
            {
                SwitchPlayer();
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
            _currentPlayer = Player.Blue;
        }
    }
}
