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

        public GameForm(int boardSize, GameMode gameMode)
        {
            InitializeComponent();
            InitializeFormSettings(boardSize, gameMode);

            _gameState = new GameState(boardSize, gameMode);
            _gridButtons = new Button[boardSize, boardSize];

            WireUpEventHandlers();
            InitializeBoard();
            UpdateUI();
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
            int spacing = 10; // Dynamic spacing between controls
            
            // Position btnPlaceS
            btnPlaceS.Location = new Point(CellMargin, controlsY);
            
            // Position btnPlaceO dynamically based on btnPlaceS
            btnPlaceO.Location = new Point(btnPlaceS.Right + spacing, controlsY);
            
            // Position lblTurn dynamically based on btnPlaceO
            lblTurn.Location = new Point(btnPlaceO.Right + spacing, controlsY + 20);
            
            // Position btnNewGame below the buttons
            btnNewGame.Location = new Point(CellMargin, controlsY + 80);
            
            // Position status labels to the right of the board (with spacing)
            int labelX = panelGameBoard.Right + 20; // 20px to the right of the board
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
            bool moveSuccessful = _gameState.MakeMove(row, col, cellValue);

            if (moveSuccessful)
            {
                UpdateCellDisplay(row, col, value);
                UpdateUI();

                if (_gameState.IsGameOver)
                {
                    HandleGameOver();
                }
            }
            else
            {
                ShowMessage("Cell already occupied!", "Invalid Move");
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

            // Both players can place either S or O - enable both buttons
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
            _gameState.Reset();
            ClearAllCells();
            UpdateUI();
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

        private void GameForm_Load(object? sender, EventArgs e)
        {
        }

        private void lblBlueScore_Click(object sender, EventArgs e)
        {

        }
    }
}