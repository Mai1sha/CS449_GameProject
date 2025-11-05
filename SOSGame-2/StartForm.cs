using SOSGame.Models;

namespace SOSGame
{
    public partial class StartForm : Form
    {
        private const int MinimumBoardSize = 3;
        private GameForm? _gameForm;

        public StartForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "SOS Game - Home";
            this.StartPosition = FormStartPosition.CenterScreen;
            simpleGameButton.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void generalGameButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            if (!TryGetBoardSize(out int boardSize))
            {
                MessageBox.Show("Please enter a valid board size (must be >= 3)", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GameMode gameMode = GetSelectedGameMode();
            StartGame(boardSize, gameMode);
        }

        private void StartGame(int boardSize, GameMode gameMode)
        {
            _gameForm = new GameForm(boardSize, gameMode);
            _gameForm.FormClosed += GameForm_FormClosed;
            _gameForm.Show();
            this.Hide();
        }

        private void GameForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _gameForm = null;
            BoardSizeTextBox.Clear();
            simpleGameButton.Checked = true;
            this.Show();
        }

        private bool TryGetBoardSize(out int boardSize)
        {
            boardSize = 0;
            string input = BoardSizeTextBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
                return false;

            if (!int.TryParse(input, out int size))
                return false;

            if (size < MinimumBoardSize)
                return false;

            boardSize = size;
            return true;
        }

        private GameMode GetSelectedGameMode()
        {
            return generalGameButton.Checked ? GameMode.General : GameMode.Simple;
        }

        private void BoardSizeTextBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}