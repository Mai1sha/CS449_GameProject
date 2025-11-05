namespace SOSGame.Models
{
    public abstract class GameLogic
    {
        protected readonly Board _board;
        protected int _blueScore;
        protected int _redScore;
        protected bool _gameOver;
        protected Player? _winner;

        public int BlueScore => _blueScore;
        public int RedScore => _redScore;
        public bool IsGameOver => _gameOver;
        public Player? Winner => _winner;

        protected GameLogic(Board board)
        {
            _board = board;
            _blueScore = 0;
            _redScore = 0;
            _gameOver = false;
            _winner = null;
        }

        public abstract List<SOSSequence> CheckForSOS(int row, int col, Player currentPlayer);
        
        public abstract bool ShouldSwitchPlayer(List<SOSSequence> sosSequences);
        
        public abstract void UpdateGameState(List<SOSSequence> sosSequences, Player currentPlayer);
        
        protected abstract void DetermineWinner();

        public virtual void Reset()
        {
            _blueScore = 0;
            _redScore = 0;
            _gameOver = false;
            _winner = null;
        }

        protected List<SOSSequence> DetectAllSOSSequences(int row, int col, Player currentPlayer)
        {
            List<SOSSequence> sequences = new List<SOSSequence>();

            CellValue placedValue = _board.GetCell(row, col);
            
            // Check for SOS patterns based on what was just placed
            if (placedValue == CellValue.S)
            {
                // S was placed - check if it's the start or end of SOS
                sequences.AddRange(CheckHorizontalSOS(row, col, currentPlayer));
                sequences.AddRange(CheckVerticalSOS(row, col, currentPlayer));
                sequences.AddRange(CheckDiagonalSOS(row, col, currentPlayer));
            }
            else if (placedValue == CellValue.O)
            {
                // O was placed - check if it's in the middle of S_S
                sequences.AddRange(CheckSOSWithOInMiddle(row, col, currentPlayer));
            }

            return sequences;
        }

        private List<SOSSequence> CheckSOSWithOInMiddle(int row, int col, Player player)
        {
            List<SOSSequence> sequences = new List<SOSSequence>();

            // Horizontal: S-O-S
            if (col >= 1 && col < _board.Size - 1 &&
                _board.GetCell(row, col - 1) == CellValue.S &&
                _board.GetCell(row, col + 1) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col - 1, row, col + 1, player));
            }

            // Vertical: S-O-S
            if (row >= 1 && row < _board.Size - 1 &&
                _board.GetCell(row - 1, col) == CellValue.S &&
                _board.GetCell(row + 1, col) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row - 1, col, row + 1, col, player));
            }

            // Diagonal (top-left to bottom-right): S-O-S
            if (row >= 1 && row < _board.Size - 1 && col >= 1 && col < _board.Size - 1 &&
                _board.GetCell(row - 1, col - 1) == CellValue.S &&
                _board.GetCell(row + 1, col + 1) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row - 1, col - 1, row + 1, col + 1, player));
            }

            // Diagonal (top-right to bottom-left): S-O-S
            if (row >= 1 && row < _board.Size - 1 && col >= 1 && col < _board.Size - 1 &&
                _board.GetCell(row - 1, col + 1) == CellValue.S &&
                _board.GetCell(row + 1, col - 1) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row + 1, col - 1, row - 1, col + 1, player));
            }

            return sequences;
        }

        private List<SOSSequence> CheckHorizontalSOS(int row, int col, Player player)
        {
            List<SOSSequence> sequences = new List<SOSSequence>();

            // Check if S is at the END of S-O-S (pattern to the left)
            if (col >= 2 && 
                _board.GetCell(row, col - 1) == CellValue.O && 
                _board.GetCell(row, col - 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col - 2, row, col, player));
            }

            // Check if S is at the START of S-O-S (pattern to the right)
            if (col <= _board.Size - 3 && 
                _board.GetCell(row, col + 1) == CellValue.O && 
                _board.GetCell(row, col + 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col, row, col + 2, player));
            }

            return sequences;
        }

        private List<SOSSequence> CheckVerticalSOS(int row, int col, Player player)
        {
            List<SOSSequence> sequences = new List<SOSSequence>();

            // Check if S is at the BOTTOM of vertical S-O-S (pattern above)
            if (row >= 2 && 
                _board.GetCell(row - 1, col) == CellValue.O && 
                _board.GetCell(row - 2, col) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row - 2, col, row, col, player));
            }

            // Check if S is at the TOP of vertical S-O-S (pattern below)
            if (row <= _board.Size - 3 && 
                _board.GetCell(row + 1, col) == CellValue.O && 
                _board.GetCell(row + 2, col) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col, row + 2, col, player));
            }

            return sequences;
        }

        private List<SOSSequence> CheckDiagonalSOS(int row, int col, Player player)
        {
            List<SOSSequence> sequences = new List<SOSSequence>();

            // Diagonal (\): Check if S is at bottom-right of S-O-S (pattern to top-left)
            if (row >= 2 && col >= 2 && 
                _board.GetCell(row - 1, col - 1) == CellValue.O && 
                _board.GetCell(row - 2, col - 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row - 2, col - 2, row, col, player));
            }

            // Diagonal (\): Check if S is at top-left of S-O-S (pattern to bottom-right)
            if (row <= _board.Size - 3 && col <= _board.Size - 3 && 
                _board.GetCell(row + 1, col + 1) == CellValue.O && 
                _board.GetCell(row + 2, col + 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col, row + 2, col + 2, player));
            }

            // Diagonal (/): Check if S is at bottom-left of S-O-S (pattern to top-right)
            if (row >= 2 && col <= _board.Size - 3 && 
                _board.GetCell(row - 1, col + 1) == CellValue.O && 
                _board.GetCell(row - 2, col + 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row - 2, col + 2, row, col, player));
            }

            // Diagonal (/): Check if S is at top-right of S-O-S (pattern to bottom-left)
            if (row <= _board.Size - 3 && col >= 2 && 
                _board.GetCell(row + 1, col - 1) == CellValue.O && 
                _board.GetCell(row + 2, col - 2) == CellValue.S)
            {
                sequences.Add(new SOSSequence(row, col, row + 2, col - 2, player));
            }

            return sequences;
        }
    }
}
