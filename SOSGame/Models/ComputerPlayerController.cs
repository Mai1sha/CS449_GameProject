namespace SOSGame.Models
{
    /// <summary>
    /// Controller for computer players. Implements basic AI strategies for both Simple and General game modes.
    /// Strategy hierarchy:
    /// 1. Try to make SOS (winning move in Simple, scoring move in General)
    /// 2. Try to block opponent from making SOS
    /// 3. Make a strategic move that sets up future SOS opportunities
    /// 4. Make a random valid move
    /// </summary>
    public class ComputerPlayerController : PlayerController
    {
        private readonly Random _random;

        public ComputerPlayerController(Player player) : base(player, PlayerType.Computer)
        {
            _random = new Random();
        }

        public override Move? GetMove(Board board, GameLogic gameLogic, GameMode gameMode)
        {
            Move? sosMove = FindMoveToCreateSOS(board);
            if (sosMove != null)
                return sosMove;

            Move? blockMove = FindMoveToBlockSOS(board);
            if (blockMove != null)
                return blockMove;

            Move? setupMove = FindStrategicSetupMove(board);
            if (setupMove != null)
                return setupMove;

            return GetRandomMove(board);
        }

        /// <summary>
        /// Finds a move that immediately creates an SOS sequence.
        /// </summary>
        private Move? FindMoveToCreateSOS(Board board)
        {
            List<Move> allPossibleMoves = GetAllPossibleMoves(board);
            allPossibleMoves = allPossibleMoves.OrderBy(x => _random.Next()).ToList();

            foreach (var move in allPossibleMoves)
            {
                if (WouldCreateSOS(board, move))
                {
                    return move;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a move that blocks the opponent from creating an SOS.
        /// </summary>
        private Move? FindMoveToBlockSOS(Board board)
        {
            List<Move> allPossibleMoves = GetAllPossibleMoves(board);

            foreach (var move in allPossibleMoves)
            {
                if (IsBlockingMove(board, move))
                {
                    return move;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a strategic setup move that creates future opportunities.
        /// Prioritizes: center positions, edge positions near existing pieces.
        /// </summary>
        private Move? FindStrategicSetupMove(Board board)
        {
            List<Move> allPossibleMoves = GetAllPossibleMoves(board);
            
            if (allPossibleMoves.Count == 0)
                return null;

            int center = board.Size / 2;

            var centerMoves = allPossibleMoves
                .Where(m => Math.Abs(m.Row - center) <= 1 && Math.Abs(m.Col - center) <= 1)
                .ToList();

            if (centerMoves.Any())
            {
                var centerO = centerMoves.Where(m => m.Value == CellValue.O).ToList();
                var centerS = centerMoves.Where(m => m.Value == CellValue.S).ToList();
                
                if (centerO.Any() && centerS.Any())
                {
                    if (_random.Next(100) < 60 && centerO.Any())
                        return centerO[_random.Next(centerO.Count)];
                    else if (centerS.Any())
                        return centerS[_random.Next(centerS.Count)];
                }
                
                return centerMoves[_random.Next(centerMoves.Count)];
            }

            var adjacentMoves = allPossibleMoves
                .Where(m => HasAdjacentPiece(board, m.Row, m.Col))
                .ToList();

            if (adjacentMoves.Any())
            {
                return adjacentMoves[_random.Next(adjacentMoves.Count)];
            }

            return null;
        }

        /// <summary>
        /// Gets a random valid move from all available moves.
        /// </summary>
        private Move? GetRandomMove(Board board)
        {
            List<Move> allPossibleMoves = GetAllPossibleMoves(board);

            if (allPossibleMoves.Count == 0)
                return null;

            return allPossibleMoves[_random.Next(allPossibleMoves.Count)];
        }

        /// <summary>
        /// Gets all possible moves (empty cells with either S or O).
        /// </summary>
        private List<Move> GetAllPossibleMoves(Board board)
        {
            List<Move> moves = new List<Move>();

            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsCellEmpty(row, col))
                    {
                        moves.Add(new Move(row, col, CellValue.S));
                        moves.Add(new Move(row, col, CellValue.O));
                    }
                }
            }

            return moves;
        }

        /// <summary>
        /// Checks if a move would create an SOS sequence.
        /// </summary>
        private bool WouldCreateSOS(Board board, Move move)
        {
            Board testBoard = CloneBoard(board);
            testBoard.PlaceMove(move.Row, move.Col, move.Value);
            return CheckForSOSAtPosition(testBoard, move.Row, move.Col);
        }

        /// <summary>
        /// Checks if a move blocks an opponent's potential SOS.
        /// </summary>
        private bool IsBlockingMove(Board board, Move move)
        {
            Move opponentMoveS = new Move(move.Row, move.Col, CellValue.S);
            Move opponentMoveO = new Move(move.Row, move.Col, CellValue.O);

            return WouldCreateSOS(board, opponentMoveS) || WouldCreateSOS(board, opponentMoveO);
        }

        /// <summary>
        /// Checks if there's an SOS sequence at the given position.
        /// </summary>
        private bool CheckForSOSAtPosition(Board board, int row, int col)
        {
            CellValue placedValue = board.GetCell(row, col);

            if (placedValue == CellValue.Empty)
                return false;

            if (placedValue == CellValue.S)
            {
                return CheckSOSPatternForS(board, row, col);
            }
            else
            {
                return CheckSOSPatternForO(board, row, col);
            }
        }

        /// <summary>
        /// Checks SOS patterns when S is placed (S can be start or end of SOS).
        /// </summary>
        private bool CheckSOSPatternForS(Board board, int row, int col)
        {
            // Check all 8 directions for S-O-S patterns

            // Horizontal: S-O-S (this S at start)
            if (col <= board.Size - 3 &&
                board.GetCell(row, col + 1) == CellValue.O &&
                board.GetCell(row, col + 2) == CellValue.S)
                return true;

            // Horizontal: S-O-S (this S at end)
            if (col >= 2 &&
                board.GetCell(row, col - 1) == CellValue.O &&
                board.GetCell(row, col - 2) == CellValue.S)
                return true;

            // Vertical: S-O-S (this S at start)
            if (row <= board.Size - 3 &&
                board.GetCell(row + 1, col) == CellValue.O &&
                board.GetCell(row + 2, col) == CellValue.S)
                return true;

            // Vertical: S-O-S (this S at end)
            if (row >= 2 &&
                board.GetCell(row - 1, col) == CellValue.O &&
                board.GetCell(row - 2, col) == CellValue.S)
                return true;

            // Diagonal \ : S-O-S (this S at start)
            if (row <= board.Size - 3 && col <= board.Size - 3 &&
                board.GetCell(row + 1, col + 1) == CellValue.O &&
                board.GetCell(row + 2, col + 2) == CellValue.S)
                return true;

            // Diagonal \ : S-O-S (this S at end)
            if (row >= 2 && col >= 2 &&
                board.GetCell(row - 1, col - 1) == CellValue.O &&
                board.GetCell(row - 2, col - 2) == CellValue.S)
                return true;

            // Diagonal / : S-O-S (this S at start - going up-right)
            if (row >= 2 && col <= board.Size - 3 &&
                board.GetCell(row - 1, col + 1) == CellValue.O &&
                board.GetCell(row - 2, col + 2) == CellValue.S)
                return true;

            // Diagonal / : S-O-S (this S at end - going down-left)
            if (row <= board.Size - 3 && col >= 2 &&
                board.GetCell(row + 1, col - 1) == CellValue.O &&
                board.GetCell(row + 2, col - 2) == CellValue.S)
                return true;

            return false;
        }

        /// <summary>
        /// Checks SOS patterns when O is placed (O must be in middle of S-O-S).
        /// </summary>
        private bool CheckSOSPatternForO(Board board, int row, int col)
        {
            // Horizontal: S-O-S
            if (col >= 1 && col < board.Size - 1 &&
                board.GetCell(row, col - 1) == CellValue.S &&
                board.GetCell(row, col + 1) == CellValue.S)
                return true;

            // Vertical: S-O-S
            if (row >= 1 && row < board.Size - 1 &&
                board.GetCell(row - 1, col) == CellValue.S &&
                board.GetCell(row + 1, col) == CellValue.S)
                return true;

            // Diagonal \ : S-O-S
            if (row >= 1 && row < board.Size - 1 && col >= 1 && col < board.Size - 1 &&
                board.GetCell(row - 1, col - 1) == CellValue.S &&
                board.GetCell(row + 1, col + 1) == CellValue.S)
                return true;

            // Diagonal / : S-O-S
            if (row >= 1 && row < board.Size - 1 && col >= 1 && col < board.Size - 1 &&
                board.GetCell(row - 1, col + 1) == CellValue.S &&
                board.GetCell(row + 1, col - 1) == CellValue.S)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if a cell has any adjacent pieces (for strategic positioning).
        /// </summary>
        private bool HasAdjacentPiece(Board board, int row, int col)
        {
            int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dRow[i];
                int newCol = col + dCol[i];

                if (newRow >= 0 && newRow < board.Size &&
                    newCol >= 0 && newCol < board.Size &&
                    board.GetCell(newRow, newCol) != CellValue.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a deep clone of the board for testing moves.
        /// </summary>
        private Board CloneBoard(Board original)
        {
            Board clone = new Board(original.Size);

            for (int row = 0; row < original.Size; row++)
            {
                for (int col = 0; col < original.Size; col++)
                {
                    CellValue value = original.GetCell(row, col);
                    if (value != CellValue.Empty)
                    {
                        clone.PlaceMove(row, col, value);
                    }
                }
            }

            return clone;
        }
    }
}
