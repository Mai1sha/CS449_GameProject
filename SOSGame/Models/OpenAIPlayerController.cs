using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOSGame.Models
{
    public class OpenAIPlayerController : PlayerController
    {
        private readonly OpenAIApiClient _apiClient;
        private readonly Random _random;
        private int _consecutiveFailures;
        private const int MAX_CONSECUTIVE_FAILURES = 3;
        
        public string? LastErrorMessage { get; private set; }
        public OpenAIErrorType? LastErrorType { get; private set; }
        public bool HasReachedFailureThreshold => _consecutiveFailures >= MAX_CONSECUTIVE_FAILURES;
        public string? LastReasoning { get; private set; }

        public OpenAIPlayerController(Player player, string apiKey)
            : base(player, PlayerType.OpenAI)
        {
            _apiClient = new OpenAIApiClient(apiKey);
            _random = new Random();
            _consecutiveFailures = 0;
        }

        public override Move? GetMove(Board board, GameLogic gameLogic, GameMode gameMode)
        {
            return GetMove(board, gameLogic, gameMode, null);
        }

        public Move? GetMove(Board board, GameLogic gameLogic, GameMode gameMode, 
            IReadOnlyList<(int row, int col, CellValue value, Player player)>? moveHistory)
        {
            try
            {
                Task<Move?> moveTask = GetMoveFromOpenAIAsync(board, gameLogic, gameMode, moveHistory);
                moveTask.Wait();
                return moveTask.Result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return GetFallbackMove(board);
            }
        }

        public async Task<Move?> GetMoveAsync(Board board, GameLogic gameLogic, GameMode gameMode)
        {
            return await GetMoveAsync(board, gameLogic, gameMode, null);
        }

        public async Task<Move?> GetMoveAsync(Board board, GameLogic gameLogic, GameMode gameMode,
            IReadOnlyList<(int row, int col, CellValue value, Player player)>? moveHistory)
        {
            try
            {
                return await GetMoveFromOpenAIAsync(board, gameLogic, gameMode, moveHistory);
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return GetFallbackMove(board);
            }
        }

        private async Task<Move?> GetMoveFromOpenAIAsync(Board board, GameLogic gameLogic, GameMode gameMode,
            IReadOnlyList<(int row, int col, CellValue value, Player player)>? moveHistory)
        {
            Console.WriteLine("[OpenAIPlayerController] Requesting move from OpenAI API...");
            
            string prompt = BuildPrompt(board, gameLogic, gameMode, moveHistory);
            
            string response = await _apiClient.GenerateContentAsync(prompt);
            Console.WriteLine($"[OpenAIPlayerController] Received response: {response}");
            
            Move? move = ParseOpenAIResponse(response, board);
            
            if (move == null)
            {
                Console.WriteLine("[OpenAIPlayerController] Failed to parse response, using fallback");
                _consecutiveFailures++;
                return GetFallbackMove(board);
            }
            
            var (isValid, errorMessage) = ValidateMove(move, board);
            
            if (!isValid)
            {
                Console.WriteLine($"[OpenAIPlayerController] Invalid move: {errorMessage}, using fallback");
                _consecutiveFailures++;
                return GetFallbackMove(board);
            }
            
            Console.WriteLine($"[OpenAIPlayerController] Valid move: Row {move.Row}, Col {move.Col}, Letter {move.Value}");
            _consecutiveFailures = 0;
            return move;
        }

        private Move? GetFallbackMove(Board board)
        {
            Console.WriteLine("[OpenAIPlayerController] Generating fallback move...");
            
            List<Move> validMoves = new List<Move>();
            
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsCellEmpty(row, col))
                    {
                        validMoves.Add(new Move(row, col, CellValue.S));
                        validMoves.Add(new Move(row, col, CellValue.O));
                    }
                }
            }
            
            if (validMoves.Count == 0)
            {
                Console.WriteLine("[OpenAIPlayerController] No valid moves available");
                return null;
            }
            
            Move fallbackMove = validMoves[_random.Next(validMoves.Count)];
            Console.WriteLine($"[OpenAIPlayerController] Fallback move: Row {fallbackMove.Row}, Col {fallbackMove.Col}, Letter {fallbackMove.Value}");
            
            return fallbackMove;
        }

        private void HandleError(Exception ex)
        {
            Console.WriteLine($"[OpenAIPlayerController] Error getting move: {ex.Message}");
            _consecutiveFailures++;

            if (ex is OpenAIApiException apiEx)
            {
                if (apiEx.StatusCode == 401 || apiEx.StatusCode == 403)
                {
                    LastErrorType = OpenAIErrorType.Authentication;
                    LastErrorMessage = "Authentication failed. Your API key may be invalid or expired.";
                }
                else if (apiEx.StatusCode == 429)
                {
                    LastErrorType = OpenAIErrorType.RateLimit;
                    LastErrorMessage = "Rate limit exceeded. Please wait a moment before trying again.";
                }
                else
                {
                    LastErrorType = OpenAIErrorType.ApiError;
                    LastErrorMessage = $"API error: {apiEx.Message}";
                }
            }
            else if (ex is TaskCanceledException)
            {
                LastErrorType = OpenAIErrorType.Timeout;
                LastErrorMessage = $"Request timed out after 15 seconds. Using a random move instead.";
            }
            else if (ex is HttpRequestException)
            {
                LastErrorType = OpenAIErrorType.Network;
                LastErrorMessage = "Network error. Please check your internet connection.";
            }
            else
            {
                LastErrorType = OpenAIErrorType.Unknown;
                LastErrorMessage = $"Unexpected error: {ex.Message}";
            }

            if (_consecutiveFailures >= MAX_CONSECUTIVE_FAILURES)
            {
                Console.WriteLine($"[OpenAIPlayerController] Reached {MAX_CONSECUTIVE_FAILURES} consecutive failures. Consider switching player type.");
            }
        }

        private Move? ParseOpenAIResponse(string response, Board board)
        {
            if (string.IsNullOrWhiteSpace(response))
                return null;

            // Split response into lines to extract reasoning
            string[] lines = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
            // First line is reasoning, last line should be the move
            if (lines.Length >= 2)
            {
                LastReasoning = lines[0].Trim();
                response = lines[lines.Length - 1].Trim(); // Use last line for move parsing
            }
            else if (lines.Length == 1)
            {
                LastReasoning = null;
                response = lines[0].Trim();
            }
            else
            {
                LastReasoning = null;
            }

            response = response.ToUpper();

            int row = -1;
            int col = -1;
            char letter = '\0';

            var patterns = new[]
            {
                @"ROW[:\s]*(\d+)[,\s]*COL(?:UMN)?[:\s]*(\d+)[,\s]*([SO])",
                @"\((\d+)[,\s]*(\d+)\)[,\s]*([SO])",
                @"(\d+)[,\s]+(\d+)[,\s]+([SO])",
                @"R[:\s]*(\d+)[,\s]*C[:\s]*(\d+)[,\s]*([SO])",
                @"(\d+)\s+(\d+)\s+([SO])"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(response, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (int.TryParse(match.Groups[1].Value, out row) &&
                        int.TryParse(match.Groups[2].Value, out col) &&
                        match.Groups[3].Value.Length == 1)
                    {
                        letter = match.Groups[3].Value[0];
                        break;
                    }
                }
            }

            if (row == -1 || col == -1 || (letter != 'S' && letter != 'O'))
                return null;

            // Don't auto-adjust coordinates - use them as-is (0-indexed)
            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size)
                return null;

            CellValue cellValue = letter == 'S' ? CellValue.S : CellValue.O;
            return new Move(row, col, cellValue);
        }

        private string BuildPrompt(Board board, GameLogic gameLogic, GameMode gameMode,
            IReadOnlyList<(int row, int col, CellValue value, Player player)>? moveHistory)
        {
            StringBuilder prompt = new StringBuilder();
            
            // Game mode and objective
            prompt.Append("=== GAME INFORMATION ===\n");
            if (gameMode == GameMode.Simple)
            {
                prompt.Append("Mode: SIMPLE - First to create ANY S-O-S wins immediately!\n");
            }
            else
            {
                prompt.Append("Mode: GENERAL - Most S-O-S sequences wins when board is full\n");
            }
            
            // Player info and score
            string playerColor = _player == Player.Blue ? "Blue" : "Red";
            string opponentColor = _player == Player.Blue ? "Red" : "Blue";
            int myScore = _player == Player.Blue ? gameLogic.BlueScore : gameLogic.RedScore;
            int opponentScore = _player == Player.Blue ? gameLogic.RedScore : gameLogic.BlueScore;
            
            prompt.Append($"You: {playerColor} | Opponent: {opponentColor}\n");
            prompt.Append($"Score: You={myScore}, Opponent={opponentScore}\n");
            
            // Board state
            prompt.Append($"\n=== BOARD STATE ({board.Size}x{board.Size}) ===\n");
            prompt.Append("Coordinates are 0-indexed (0 to " + (board.Size - 1) + ")\n");
            prompt.Append("   ");
            for (int col = 0; col < board.Size; col++)
            {
                prompt.Append($"{col} ");
            }
            prompt.Append("\n");
            
            for (int row = 0; row < board.Size; row++)
            {
                prompt.Append($"{row}: ");
                for (int col = 0; col < board.Size; col++)
                {
                    CellValue cell = board.GetCell(row, col);
                    char cellChar = cell switch
                    {
                        CellValue.S => 'S',
                        CellValue.O => 'O',
                        _ => '.'
                    };
                    prompt.Append(cellChar);
                    prompt.Append(" ");
                }
                prompt.Append("\n");
            }
            
            // Move history (last 5 moves for context)
            if (moveHistory != null && moveHistory.Count > 0)
            {
                prompt.Append($"\n=== RECENT MOVES ===\n");
                int startIndex = Math.Max(0, moveHistory.Count - 5);
                for (int i = startIndex; i < moveHistory.Count; i++)
                {
                    var move = moveHistory[i];
                    string playerName = move.player == Player.Blue ? "Blue" : "Red";
                    char letter = move.value == CellValue.S ? 'S' : 'O';
                    prompt.Append($"{i + 1}. {playerName}: ({move.row},{move.col})={letter}\n");
                }
            }
            
            // Analyze and provide game context
            var analysis = AnalyzeBoard(board);
            
            prompt.Append("\n=== ANALYSIS ===\n");
            
            if (analysis.WinningMoves.Count > 0)
            {
                prompt.Append("WINNING MOVES (complete SOS now):\n");
                foreach (var move in analysis.WinningMoves.Take(3))
                {
                    prompt.Append($"  - ({move.row},{move.col}) = {move.letter}\n");
                }
            }
            else if (analysis.BlockingMoves.Count > 0)
            {
                prompt.Append("BLOCKING MOVES (prevent opponent SOS):\n");
                foreach (var move in analysis.BlockingMoves.Take(3))
                {
                    prompt.Append($"  - ({move.row},{move.col}) = {move.letter}\n");
                }
            }
            else if (analysis.SetupMoves.Count > 0)
            {
                prompt.Append("SETUP MOVES (create future opportunities):\n");
                foreach (var move in analysis.SetupMoves.Take(3))
                {
                    prompt.Append($"  - ({move.row},{move.col}) = {move.letter}\n");
                }
            }
            else
            {
                prompt.Append("No immediate threats or opportunities detected.\n");
                prompt.Append("Consider placing near existing pieces to create future SOS patterns.\n");
            }
            
            prompt.Append("\n=== YOUR MOVE ===\n");
            prompt.Append("Provide your reasoning (one sentence) and then your move.\n");
            prompt.Append("Format:\n");
            prompt.Append("[Your reasoning here]\n");
            prompt.Append("Row Col Letter\n");
            
            return prompt.ToString();
        }
        
        private (List<(int row, int col, char letter)> WinningMoves, 
                 List<(int row, int col, char letter)> BlockingMoves,
                 List<(int row, int col, char letter)> SetupMoves) AnalyzeBoard(Board board)
        {
            var winningMoves = new List<(int row, int col, char letter)>();
            var blockingMoves = new List<(int row, int col, char letter)>();
            var setupMoves = new List<(int row, int col, char letter)>();
            
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (!board.IsCellEmpty(row, col))
                        continue;
                    
                    // Check if placing S creates SOS for us (winning move)
                    if (WouldCreateSOS(board, row, col, CellValue.S))
                    {
                        winningMoves.Add((row, col, 'S'));
                    }
                    
                    // Check if placing O creates SOS for us (winning move)
                    if (WouldCreateSOS(board, row, col, CellValue.O))
                    {
                        winningMoves.Add((row, col, 'O'));
                    }
                    
                    // If not a winning move, check if it blocks opponent
                    // (opponent could complete SOS here on their next turn)
                    if (winningMoves.Count == 0 || !winningMoves.Any(m => m.row == row && m.col == col))
                    {
                        if (WouldCreateSOS(board, row, col, CellValue.S))
                            blockingMoves.Add((row, col, 'S'));
                        if (WouldCreateSOS(board, row, col, CellValue.O))
                            blockingMoves.Add((row, col, 'O'));
                    }
                    
                    // Check for setup moves (only if not winning or blocking)
                    if (winningMoves.Count == 0 && blockingMoves.Count == 0 && IsSetupMove(board, row, col))
                    {
                        // Prefer S near O, and O near S for better setup
                        char preferredLetter = HasAdjacentO(board, row, col) ? 'S' : 'O';
                        setupMoves.Add((row, col, preferredLetter));
                    }
                }
            }
            
            return (winningMoves, blockingMoves, setupMoves);
        }
        
        private bool WouldCreateSOS(Board board, int row, int col, CellValue value)
        {
            Board testBoard = CloneBoard(board);
            testBoard.PlaceMove(row, col, value);
            return CheckForSOSAtPosition(testBoard, row, col);
        }
        
        private bool CheckForSOSAtPosition(Board board, int row, int col)
        {
            CellValue placedValue = board.GetCell(row, col);
            if (placedValue == CellValue.Empty) return false;
            
            int[][] directions = new int[][]
            {
                new int[] {0, 1},   // Horizontal right
                new int[] {1, 0},   // Vertical down
                new int[] {1, 1},   // Diagonal down-right
                new int[] {1, -1}   // Diagonal down-left
            };
            
            foreach (var dir in directions)
            {
                // Check if this position is part of SOS in this direction
                for (int offset = -2; offset <= 0; offset++)
                {
                    int r1 = row + offset * dir[0];
                    int c1 = col + offset * dir[1];
                    int r2 = r1 + dir[0];
                    int c2 = c1 + dir[1];
                    int r3 = r2 + dir[0];
                    int c3 = c2 + dir[1];
                    
                    if (r1 >= 0 && r1 < board.Size && c1 >= 0 && c1 < board.Size &&
                        r3 >= 0 && r3 < board.Size && c3 >= 0 && c3 < board.Size)
                    {
                        if (board.GetCell(r1, c1) == CellValue.S &&
                            board.GetCell(r2, c2) == CellValue.O &&
                            board.GetCell(r3, c3) == CellValue.S)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        
        private bool IsSetupMove(Board board, int row, int col)
        {
            return HasAdjacentPiece(board, row, col);
        }
        
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
        
        private bool HasAdjacentO(Board board, int row, int col)
        {
            int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };
            
            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dRow[i];
                int newCol = col + dCol[i];
                
                if (newRow >= 0 && newRow < board.Size &&
                    newCol >= 0 && newCol < board.Size &&
                    board.GetCell(newRow, newCol) == CellValue.O)
                {
                    return true;
                }
            }
            
            return false;
        }
        
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

        private (bool isValid, string? errorMessage) ValidateMove(Move move, Board board)
        {
            if (move.Row < 0 || move.Row >= board.Size || move.Col < 0 || move.Col >= board.Size)
            {
                return (false, $"Coordinates ({move.Row}, {move.Col}) are out of bounds for board size {board.Size}");
            }

            if (!board.IsCellEmpty(move.Row, move.Col))
            {
                return (false, $"Cell at ({move.Row}, {move.Col}) is already occupied");
            }

            if (move.Value != CellValue.S && move.Value != CellValue.O)
            {
                return (false, $"Invalid letter '{move.Value}'. Must be 'S' or 'O'");
            }

            return (true, null);
        }
    }

    public enum OpenAIErrorType
    {
        Authentication,
        Timeout,
        Network,
        RateLimit,
        ApiError,
        Unknown
    }
}
