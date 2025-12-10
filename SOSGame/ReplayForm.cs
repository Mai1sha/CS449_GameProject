using System;
using System.Drawing;
using System.Windows.Forms;
using SOSGame.Models;

namespace SOSGame
{
    public partial class ReplayForm : Form
    {
        private const int CellSize = 80;
        private const int CellMargin = 15;

        private readonly GameReplayer _replayer;
        private Board _board = null!;
        private Button[,] _gridButtons = null!;

        public ReplayForm(string filePath)
        {
            InitializeComponent();
            
            _replayer = new GameReplayer();
            
            try
            {
                // Load game file
                GameRecordingData recordingData = _replayer.LoadFromFile(filePath);
                
                // Initialize board with the recorded board size
                _board = new Board(recordingData.BoardSize);
                _gridButtons = new Button[recordingData.BoardSize, recordingData.BoardSize];
                
                // Set form title
                this.Text = $"Game Replay - {recordingData.GameMode} ({recordingData.Timestamp:yyyy-MM-dd HH:mm})";
                this.StartPosition = FormStartPosition.CenterScreen;
                
                // Wire up event handlers
                WireUpEventHandlers();
                
                // Initialize the board display
                InitializeBoard();
                
                // Display game metadata
                DisplayGameMetadata();
                
                // Update UI to initial state
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game file:\n{ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void WireUpEventHandlers()
        {
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            btnReset.Click += btnReset_Click;
        }

        private void InitializeBoard()
        {
            int boardSize = _board.Size;
            int boardWidth = CalculateBoardDimension(boardSize);
            int boardHeight = CalculateBoardDimension(boardSize);

            ConfigureGamePanel(boardWidth, boardHeight);
            CreateGridButtons(boardSize);
        }

        private int CalculateBoardDimension(int boardSize)
        {
            return boardSize * CellSize + (boardSize + 1) * CellMargin;
        }

        private void ConfigureGamePanel(int boardWidth, int boardHeight)
        {
            panelGameBoard.Size = new Size(boardWidth, boardHeight);
            panelGameBoard.Location = new Point(CellMargin, CellMargin);
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
                Enabled = false, // Disable all cell buttons (no user input)
                BackColor = SystemColors.Control
            };

            panelGameBoard.Controls.Add(button);
            _gridButtons[row, col] = button;
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            RecordedMove? move = _replayer.GetNextMove();
            if (move != null)
            {
                // Place the move on the board
                _board.PlaceMove(move.Row, move.Col, move.Value);
                
                // Update the display
                UpdateBoardDisplay();
                UpdateUI();
            }
        }

        private void btnPrevious_Click(object? sender, EventArgs e)
        {
            _replayer.GetPreviousMove();
            
            // Reset board and replay all moves up to current position
            _board.Reset();
            List<RecordedMove> movesToReplay = _replayer.GetMovesUpToCurrent();
            
            foreach (RecordedMove move in movesToReplay)
            {
                _board.PlaceMove(move.Row, move.Col, move.Value);
            }
            
            // Update the display
            UpdateBoardDisplay();
            UpdateUI();
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            _replayer.Reset();
            _board.Reset();
            
            // Update the display
            UpdateBoardDisplay();
            UpdateUI();
        }

        private void UpdateNavigationButtons()
        {
            btnPrevious.Enabled = _replayer.HasPreviousMove;
            btnNext.Enabled = _replayer.HasNextMove;
        }

        private void UpdateBoardDisplay()
        {
            // Update all cell buttons to reflect current board state
            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    CellValue cellValue = _board.GetCell(row, col);
                    
                    if (cellValue == CellValue.Empty)
                    {
                        _gridButtons[row, col].Text = "";
                    }
                    else if (cellValue == CellValue.S)
                    {
                        _gridButtons[row, col].Text = "S";
                    }
                    else if (cellValue == CellValue.O)
                    {
                        _gridButtons[row, col].Text = "O";
                    }
                }
            }
        }

        private void DisplayGameMetadata()
        {
            if (_replayer.RecordingData == null)
                return;

            GameRecordingData data = _replayer.RecordingData;
            
            lblBoardSize.Text = $"Board: {data.BoardSize}x{data.BoardSize}";
            lblGameMode.Text = $"Mode: {data.GameMode}";
            lblPlayerTypes.Text = $"Players: Blue ({data.BluePlayerType}) vs Red ({data.RedPlayerType})";
        }

        private void UpdateMoveInfo()
        {
            // Update move counter
            lblMoveCounter.Text = $"Move: {_replayer.CurrentMoveIndex} / {_replayer.TotalMoves}";
            
            // Update current move details
            if (_replayer.CurrentMoveIndex > 0 && _replayer.RecordingData != null)
            {
                RecordedMove lastMove = _replayer.RecordingData.Moves[_replayer.CurrentMoveIndex - 1];
                lblCurrentMove.Text = $"Last Move: {lastMove.Player} placed '{lastMove.Value}' at ({lastMove.Row}, {lastMove.Col})";
                lblCurrentMove.ForeColor = lastMove.Player == Player.Blue ? Color.Blue : Color.Red;
            }
            else
            {
                lblCurrentMove.Text = "No moves yet";
                lblCurrentMove.ForeColor = Color.Black;
            }
        }

        private void UpdateGameOutcome()
        {
            if (_replayer.RecordingData == null)
                return;

            // Show final outcome when replay is complete
            if (_replayer.CurrentMoveIndex >= _replayer.TotalMoves)
            {
                GameRecordingData data = _replayer.RecordingData;
                
                if (data.Winner.HasValue)
                {
                    lblFinalOutcome.Text = $"Winner: {data.Winner.Value}\nBlue: {data.BlueScore} | Red: {data.RedScore}";
                    lblFinalOutcome.ForeColor = data.Winner.Value == Player.Blue ? Color.Blue : Color.Red;
                }
                else
                {
                    lblFinalOutcome.Text = $"Draw!\nBlue: {data.BlueScore} | Red: {data.RedScore}";
                    lblFinalOutcome.ForeColor = Color.Black;
                }
            }
            else
            {
                lblFinalOutcome.Text = "";
            }
        }

        private void UpdateUI()
        {
            UpdateNavigationButtons();
            UpdateMoveInfo();
            UpdateGameOutcome();
        }
    }
}
