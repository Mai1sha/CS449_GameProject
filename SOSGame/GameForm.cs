using System;
using System.Drawing;
using System.Windows.Forms;
using SOSGame.Models;

namespace SOSGame
{
    public partial class GameForm : Form
    {
        private const int CellSize = 80;
        private const int CellMargin = 15;
        private const int MinControlWidth = 700;
        private const int ControlsHeight = 200;

        private readonly GameState _gameState;
        private readonly Button[,] _gridButtons;
        private System.Windows.Forms.Timer? _computerMoveTimer;
        private bool _processingComputerMove;
        private readonly bool _recordingEnabled;
        private GameRecorder? _gameRecorder;

        public GameForm(int boardSize, GameMode gameMode, PlayerType bluePlayerType = PlayerType.Human, 
            PlayerType redPlayerType = PlayerType.Human, bool enableRecording = false)
        {
            InitializeComponent();
            InitializeFormSettings(boardSize, gameMode);

            _gameState = new GameState(boardSize, gameMode, bluePlayerType, redPlayerType);
            _gridButtons = new Button[boardSize, boardSize];
            _processingComputerMove = false;
            _recordingEnabled = enableRecording;

            if (_recordingEnabled)
            {
                _gameRecorder = new GameRecorder();
                _gameRecorder.StartRecording(boardSize, gameMode, bluePlayerType, redPlayerType);
            }

            WireUpEventHandlers();
            InitializeBoard();
            UpdateUI();

            if (_gameState.CurrentPlayerController.IsComputer() || _gameState.CurrentPlayerController.IsOpenAI())
            {
                InitiateComputerMove();
            }
        }

        private void InitializeFormSettings(int boardSize, GameMode gameMode)
        {
            this.Text = $"SOS Game - {gameMode} ({boardSize}x{boardSize})";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void WireUpEventHandlers()
        {
            btnPlaceS.Click += btnPlaceS_Click;
            btnPlaceO.Click += btnPlaceO_Click;
            btnNewGame.Click += btnNewGame_Click;
        }

        private void InitializeBoard()
        {
            int boardSize = _gameState.Board.Size;
            int boardWidth = CalculateBoardDimension(boardSize);
            int boardHeight = CalculateBoardDimension(boardSize);

            ResizeForm(boardWidth, boardHeight);
            ConfigureGamePanel(boardWidth, boardHeight);
            PositionControlButtons(boardHeight);
            CreateGridButtons(boardSize);
        }

        private int CalculateBoardDimension(int boardSize)
        {
            return boardSize * CellSize + (boardSize + 1) * CellMargin;
        }

        private void ResizeForm(int boardWidth, int boardHeight)
        {
            int labelAreaWidth = 200; // Extra width for labels on the right
            int totalWidth = boardWidth + labelAreaWidth + CellMargin;
            
            this.ClientSize = new Size(
                Math.Max(totalWidth, MinControlWidth),
                boardHeight + ControlsHeight
            );
        }

        private void ConfigureGamePanel(int boardWidth, int boardHeight)
        {
            panelGameBoard.Size = new Size(boardWidth, boardHeight);
            panelGameBoard.Location = new Point(CellMargin, CellMargin);
        }

        private void PositionControlButtons(int boardHeight)
        {
            int controlsY = panelGameBoard.Bottom + 20;
            int spacing = 10;
            
            btnPlaceS.Location = new Point(CellMargin, controlsY);
            btnPlaceO.Location = new Point(btnPlaceS.Right + spacing, controlsY);
            lblTurn.Location = new Point(btnPlaceO.Right + spacing, controlsY + 20);
            btnNewGame.Location = new Point(CellMargin, controlsY + 80);
            
            int labelX = panelGameBoard.Right + 20;
            int topLabelY = panelGameBoard.Top;
            
            lblGameStatus.Location = new Point(labelX, topLabelY);
            lblBlueScore.Location = new Point(labelX, topLabelY + 40);
            lblRedScore.Location = new Point(labelX, topLabelY + 70);
        }

        private void CreateGridButtons(int boardSize)
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    CreateCellButton(row, col);
                }
            }
        }

        private void CreateCellButton(int row, int col)
        {
            Button button = new Button
            {
                Width = CellSize,
                Height = CellSize,
                Left = CellMargin + col * (CellSize + CellMargin),
                Top = CellMargin + row * (CellSize + CellMargin),
                Font = new Font("Arial", 16, FontStyle.Bold),
                Tag = (row, col)
            };

            button.Click += (s, e) => CellButton_Click(row, col);
            panelGameBoard.Controls.Add(button);
            _gridButtons[row, col] = button;
        }

        private void CellButton_Click(int row, int col)
        {
            ClearPreviousSelection();
            HighlightSelectedCell(row, col);
        }

        private void ClearPreviousSelection()
        {
            for (int r = 0; r < _gameState.Board.Size; r++)
            {
                for (int c = 0; c < _gameState.Board.Size; c++)
                {
                    if (_gridButtons[r, c].BackColor == Color.Yellow)
                        _gridButtons[r, c].BackColor = SystemColors.Control;
                }
            }
        }

        private void HighlightSelectedCell(int row, int col)
        {
            _gridButtons[row, col].BackColor = Color.Yellow;
        }

        private void btnPlaceS_Click(object? sender, EventArgs e)
        {
            PlaceMove('S');
        }

        private void btnPlaceO_Click(object? sender, EventArgs e)
        {
            PlaceMove('O');
        }

        private void PlaceMove(char value)
        {
            (int row, int col)? selectedCell = FindSelectedCell();

            if (!selectedCell.HasValue)
            {
                ShowMessage("Please select a cell first!", "No Cell Selected");
                return;
            }

            TryPlaceMoveOnCell(selectedCell.Value.row, selectedCell.Value.col, value);
        }

        private (int row, int col)? FindSelectedCell()
        {
            for (int row = 0; row < _gameState.Board.Size; row++)
            {
                for (int col = 0; col < _gameState.Board.Size; col++)
                {
                    if (_gridButtons[row, col].BackColor == Color.Yellow)
                        return (row, col);
                }
            }
            return null;
        }

        private void TryPlaceMoveOnCell(int row, int col, char value)
        {
            CellValue cellValue = value == 'S' ? CellValue.S : CellValue.O;
            Player currentPlayer = _gameState.CurrentPlayer;
            bool moveSuccessful = _gameState.MakeMove(row, col, cellValue);

            if (moveSuccessful)
            {
                // Record the move if recording is enabled
                if (_gameRecorder?.IsRecording == true)
                {
                    try
                    {
                        _gameRecorder.RecordMove(row, col, cellValue, currentPlayer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to record move:\n{ex.Message}", "Recording Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                UpdateCellDisplay(row, col, value);
                ClearPreviousSelection(); // Clear selection after successful move
                UpdateUI();

                if (_gameState.IsGameOver)
                {
                    HandleGameOver();
                }
                else if (_gameState.CurrentPlayerController.IsComputer() || _gameState.CurrentPlayerController.IsOpenAI())
                {
                    InitiateComputerMove();
                }
            }
            else
            {
                ShowMessage("Cell already occupied!", "Invalid Move");
            }
        }

        private void InitiateComputerMove()
        {
            if (_processingComputerMove || _gameState.IsGameOver)
                return;

            _processingComputerMove = true;

            btnPlaceS.Enabled = false;
            btnPlaceO.Enabled = false;

            if (_gameState.CurrentPlayerController.IsOpenAI())
            {
                string playerName = _gameState.CurrentPlayer == Player.Blue ? "Blue" : "Red";
                lblTurn.Text = $"OpenAI ({playerName}) is thinking...";
                lblLoadingIndicator.Visible = true;
                
                Task.Run(async () =>
                {
                    try
                    {
                        await ExecuteOpenAIMoveAsync();
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            lblLoadingIndicator.Visible = false;
                            MessageBox.Show($"Error during OpenAI move:\n{ex.Message}", "AI Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _processingComputerMove = false;
                            UpdateUI();
                        });
                    }
                });
            }
            else
            {
                string playerName = _gameState.CurrentPlayer == Player.Blue ? "Blue" : "Red";
                lblTurn.Text = $"Computer ({playerName}) is thinking...";

                _computerMoveTimer = new System.Windows.Forms.Timer();
                _computerMoveTimer.Interval = 500;
                _computerMoveTimer.Tick += ComputerMoveTimer_Tick;
                _computerMoveTimer.Start();
            }
        }

        private void ComputerMoveTimer_Tick(object? sender, EventArgs e)
        {
            _computerMoveTimer?.Stop();
            _computerMoveTimer?.Dispose();
            _computerMoveTimer = null;

            ExecuteComputerMove();
        }

        private async Task ExecuteOpenAIMoveAsync()
        {
            if (_gameState.CurrentPlayerController is OpenAIPlayerController openAIController)
            {
                Move? openAIMove = await openAIController.GetMoveAsync(
                    _gameState.Board, 
                    _gameState.GameLogic, 
                    _gameState.Mode,
                    _gameState.MoveHistory);

                this.Invoke((MethodInvoker)delegate
                {
                    lblLoadingIndicator.Visible = false;

                    if (openAIController.LastErrorType.HasValue)
                    {
                        DisplayOpenAIError(openAIController);
                    }

                    if (openAIMove != null)
                    {
                        // Display AI reasoning if available
                        if (!string.IsNullOrEmpty(openAIController.LastReasoning))
                        {
                            string playerName = _gameState.CurrentPlayer == Player.Blue ? "Blue" : "Red";
                            char letter = openAIMove.Value == CellValue.S ? 'S' : 'O';
                            
                            lblAIReasoning.Text = $"🤖 AI ({playerName}): \"{openAIController.LastReasoning}\" → Move: ({openAIMove.Row},{openAIMove.Col})={letter}";
                            lblAIReasoning.Visible = true;
                        }

                        Player currentPlayer = _gameState.CurrentPlayer;
                        bool moveSuccessful = _gameState.MakeMove(openAIMove.Row, openAIMove.Col, openAIMove.Value);

                        if (moveSuccessful)
                        {
                            if (_gameRecorder?.IsRecording == true)
                            {
                                try
                                {
                                    _gameRecorder.RecordMove(openAIMove.Row, openAIMove.Col, openAIMove.Value, currentPlayer);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Failed to record move:\n{ex.Message}", "Recording Error", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }

                            char displayValue = openAIMove.Value == CellValue.S ? 'S' : 'O';
                            UpdateCellDisplay(openAIMove.Row, openAIMove.Col, displayValue);
                            UpdateUI();

                            if (_gameState.IsGameOver)
                            {
                                HandleGameOver();
                                _processingComputerMove = false;
                            }
                            else if (_gameState.CurrentPlayerController.IsComputer() || _gameState.CurrentPlayerController.IsOpenAI())
                            {
                                _processingComputerMove = false;
                                InitiateComputerMove();
                            }
                            else
                            {
                                _processingComputerMove = false;
                            }
                        }
                        else
                        {
                            _processingComputerMove = false;
                            UpdateUI();
                        }
                    }
                    else
                    {
                        _processingComputerMove = false;
                        UpdateUI();
                    }
                });
            }
        }

        private void ExecuteComputerMove()
        {
            Move? computerMove = _gameState.GetComputerMove();

            if (computerMove != null)
            {
                Player currentPlayer = _gameState.CurrentPlayer;
                bool moveSuccessful = _gameState.MakeMove(computerMove.Row, computerMove.Col, computerMove.Value);

                if (moveSuccessful)
                {
                    if (_gameRecorder?.IsRecording == true)
                    {
                        try
                        {
                            _gameRecorder.RecordMove(computerMove.Row, computerMove.Col, computerMove.Value, currentPlayer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to record move:\n{ex.Message}", "Recording Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    char displayValue = computerMove.Value == CellValue.S ? 'S' : 'O';
                    UpdateCellDisplay(computerMove.Row, computerMove.Col, displayValue);
                    UpdateUI();

                    if (_gameState.IsGameOver)
                    {
                        HandleGameOver();
                        _processingComputerMove = false;
                    }
                    else if (_gameState.CurrentPlayerController.IsComputer() || _gameState.CurrentPlayerController.IsOpenAI())
                    {
                        _processingComputerMove = false;
                        InitiateComputerMove();
                    }
                    else
                    {
                        _processingComputerMove = false;
                    }
                }
                else
                {
                    _processingComputerMove = false;
                    UpdateUI();
                }
            }
            else
            {
                _processingComputerMove = false;
                UpdateUI();
            }
        }

        private void UpdateCellDisplay(int row, int col, char value)
        {
            _gridButtons[row, col].Text = value.ToString();
            _gridButtons[row, col].BackColor = SystemColors.Control;
        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void UpdateUI()
        {
            bool isBluePlayer = _gameState.CurrentPlayer == Player.Blue;

            lblTurn.Text = $"Current turn: {_gameState.CurrentPlayer}";
            lblTurn.ForeColor = isBluePlayer ? Color.Blue : Color.Red;

            lblBlueScore.Text = $"Blue: {_gameState.BlueScore}";
            lblRedScore.Text = $"Red: {_gameState.RedScore}";

            if (_gameState.IsGameOver)
            {
                if (_gameState.Winner.HasValue)
                {
                    lblGameStatus.Text = $"Game Over - Winner: {_gameState.Winner.Value}";
                    lblGameStatus.ForeColor = _gameState.Winner.Value == Player.Blue ? Color.Blue : Color.Red;
                }
                else
                {
                    lblGameStatus.Text = "Game Over - Draw!";
                    lblGameStatus.ForeColor = Color.Black;
                }
            }
            else
            {
                lblGameStatus.Text = $"Game: Active ({_gameState.Mode})";
                lblGameStatus.ForeColor = Color.DarkGreen;
            }

            UpdateButtonStates(isBluePlayer);
        }

        private void UpdateButtonStates(bool isBluePlayer)
        {
            if (_gameState.IsGameOver)
            {
                btnPlaceS.Enabled = false;
                btnPlaceO.Enabled = false;
                btnPlaceS.BackColor = Color.LightGray;
                btnPlaceO.BackColor = Color.LightGray;
                return;
            }

            if (_gameState.CurrentPlayerController.IsComputer())
            {
                btnPlaceS.Enabled = false;
                btnPlaceO.Enabled = false;
                btnPlaceS.BackColor = Color.LightGray;
                btnPlaceO.BackColor = Color.LightGray;
                btnPlaceS.Text = "Place S\n(Computer)";
                btnPlaceO.Text = "Place O\n(Computer)";
                return;
            }

            if (_gameState.CurrentPlayerController.IsOpenAI())
            {
                btnPlaceS.Enabled = false;
                btnPlaceO.Enabled = false;
                btnPlaceS.BackColor = Color.LightGray;
                btnPlaceO.BackColor = Color.LightGray;
                btnPlaceS.Text = "Place S\n(OpenAI)";
                btnPlaceO.Text = "Place O\n(OpenAI)";
                return;
            }

            btnPlaceS.Enabled = true;
            btnPlaceO.Enabled = true;

            if (isBluePlayer)
            {
                btnPlaceS.BackColor = Color.LightBlue;
                btnPlaceO.BackColor = Color.LightBlue;
                btnPlaceS.Text = "Place S\n(Blue)";
                btnPlaceO.Text = "Place O\n(Blue)";
            }
            else
            {
                btnPlaceS.BackColor = Color.LightCoral;
                btnPlaceO.BackColor = Color.LightCoral;
                btnPlaceS.Text = "Place S\n(Red)";
                btnPlaceO.Text = "Place O\n(Red)";
            }
        }

        private void HandleGameOver()
        {
            if (_gameRecorder?.IsRecording == true)
            {
                try
                {
                    _gameRecorder.StopRecording(_gameState.Winner, _gameState.BlueScore, _gameState.RedScore);
                    string savedPath = _gameRecorder.SaveToFile();
                    
                    MessageBox.Show($"Game recorded to:\n{savedPath}", "Recording Saved", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save game recording:\n{ex.Message}", "Recording Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            string message;
            string title = "Game Over";

            if (_gameState.Winner.HasValue)
            {
                message = $"{_gameState.Winner.Value} player wins!\n\n";
                if (_gameState.Mode == GameMode.General)
                {
                    message += $"Blue Score: {_gameState.BlueScore}\n";
                    message += $"Red Score: {_gameState.RedScore}";
                }
            }
            else
            {
                message = "Game ended in a draw!";
                if (_gameState.Mode == GameMode.General)
                {
                    message += $"\n\nBlue Score: {_gameState.BlueScore}\n";
                    message += $"Red Score: {_gameState.RedScore}";
                }
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNewGame_Click(object? sender, EventArgs e)
        {
            // Stop any ongoing computer move timer
            if (_computerMoveTimer != null)
            {
                _computerMoveTimer.Stop();
                _computerMoveTimer.Dispose();
                _computerMoveTimer = null;
            }

            _processingComputerMove = false;
            _gameState.Reset();
            ClearAllCells();
            lblAIReasoning.Visible = false;
            lblAIReasoning.Text = "";
            UpdateUI();

            if (_gameState.CurrentPlayerController.IsComputer() || _gameState.CurrentPlayerController.IsOpenAI())
            {
                InitiateComputerMove();
            }
        }

        private void ClearAllCells()
        {
            for (int row = 0; row < _gameState.Board.Size; row++)
            {
                for (int col = 0; col < _gameState.Board.Size; col++)
                {
                    _gridButtons[row, col].Text = "";
                    _gridButtons[row, col].BackColor = SystemColors.Control;
                }
            }
        }

        private void DisplayOpenAIError(OpenAIPlayerController openAIController)
        {
            string message = openAIController.LastErrorMessage ?? "An unknown error occurred.";
            string title = "OpenAI Error";
            MessageBoxIcon icon = MessageBoxIcon.Warning;

            if (openAIController.LastErrorType == OpenAIErrorType.Authentication)
            {
                title = "Authentication Error";
                icon = MessageBoxIcon.Error;
                message += "\n\nPlease check your API key configuration.";
            }
            else if (openAIController.LastErrorType == OpenAIErrorType.RateLimit)
            {
                title = "Rate Limit Exceeded";
                icon = MessageBoxIcon.Warning;
                message += "\n\nPlease wait a moment before making more requests.";
                message += "\nA random valid move was used to continue the game.";
            }
            else if (openAIController.LastErrorType == OpenAIErrorType.Timeout)
            {
                title = "Request Timeout";
                message += "\n\nA random valid move was used to continue the game.";
            }
            else if (openAIController.LastErrorType == OpenAIErrorType.Network)
            {
                title = "Network Error";
                message += "\n\nA random valid move was used to continue the game.";
            }

            if (openAIController.HasReachedFailureThreshold)
            {
                message += "\n\n⚠️ Multiple consecutive failures detected.";
                message += "\nConsider switching to a different player type or checking your API configuration.";
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }
}