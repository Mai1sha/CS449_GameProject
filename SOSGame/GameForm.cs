using System;
using System.Drawing;
using System.Windows.Forms;
using SOSGame.Models;

namespace SOSGame
{
    public partial class GameForm : Form
    {
        private const int CellSize = 50;
        private const int CellMargin = 10;
        private const int MinControlWidth = 500;
        private const int ControlsHeight = 150;
        
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
            this.ClientSize = new Size(
                Math.Max(boardWidth, MinControlWidth),
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
            btnPlaceS.Location = new Point(CellMargin, controlsY);
            btnPlaceO.Location = new Point(CellMargin + 120, controlsY);
            lblTurn.Location = new Point(CellMargin + 240, controlsY + 20);
            btnNewGame.Location = new Point(CellMargin, controlsY + 80);
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
            
            UpdateButtonStates(isBluePlayer);
        }

        private void UpdateButtonStates(bool isBluePlayer)
        {
            if (isBluePlayer)
            {
                btnPlaceS.Enabled = true;
                btnPlaceS.BackColor = Color.LightBlue;
                btnPlaceS.Text = "Place S\n(Blue)";
                
                btnPlaceO.Enabled = false;
                btnPlaceO.BackColor = Color.LightGray;
                btnPlaceO.Text = "Place O\n(Red's turn)";
            }
            else
            {
                btnPlaceS.Enabled = false;
                btnPlaceS.BackColor = Color.LightGray;
                btnPlaceS.Text = "Place S\n(Blue's turn)";
                
                btnPlaceO.Enabled = true;
                btnPlaceO.BackColor = Color.LightCoral;
                btnPlaceO.Text = "Place O\n(Red)";
            }
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
    }
}