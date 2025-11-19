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

        private void startGameButton_Click(object sender, EventArgs e)
        {
            if (!TryGetBoardSize(out int boardSize))
            {
                MessageBox.Show("Please enter a valid board size (must be >= 3)", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GameMode gameMode = GetSelectedGameMode();
            PlayerType bluePlayerType = GetBluePlayerType();
            PlayerType redPlayerType = GetRedPlayerType();
            
            StartGame(boardSize, gameMode, bluePlayerType, redPlayerType);
        }

        private void StartGame(int boardSize, GameMode gameMode, PlayerType bluePlayerType, PlayerType redPlayerType)
        {
            _gameForm = new GameForm(boardSize, gameMode, bluePlayerType, redPlayerType);
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

        private PlayerType GetBluePlayerType()
        {
            return blueComputerRadio.Checked ? PlayerType.Computer : PlayerType.Human;
        }

        private PlayerType GetRedPlayerType()
        {
            return redComputerRadio.Checked ? PlayerType.Computer : PlayerType.Human;
        }
    }
}