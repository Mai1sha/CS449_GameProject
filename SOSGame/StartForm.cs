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
            
            if ((bluePlayerType == PlayerType.OpenAI || redPlayerType == PlayerType.OpenAI) 
                && !OpenAIConfiguration.IsApiKeyConfigured())
            {
                string errorMessage = "OpenAI requires an API key to be configured.\n\n" +
                                    OpenAIConfiguration.GetConfigurationInstructions();
                MessageBox.Show(errorMessage, "API Key Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            StartGame(boardSize, gameMode, bluePlayerType, redPlayerType);
        }

        private void StartGame(int boardSize, GameMode gameMode, PlayerType bluePlayerType, PlayerType redPlayerType)
        {
            bool enableRecording = chkRecordGame.Checked;
            _gameForm = new GameForm(boardSize, gameMode, bluePlayerType, redPlayerType, enableRecording);
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
            if (blueOpenAIRadio.Checked)
                return PlayerType.OpenAI;
            if (blueComputerRadio.Checked)
                return PlayerType.Computer;
            return PlayerType.Human;
        }

        private PlayerType GetRedPlayerType()
        {
            if (redOpenAIRadio.Checked)
                return PlayerType.OpenAI;
            if (redComputerRadio.Checked)
                return PlayerType.Computer;
            return PlayerType.Human;
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Game Recording";
                openFileDialog.Filter = "SOS Game Recordings (*.sos)|*.sos|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                
                // Use RecordingsDirectoryManager to get the correct path
                string recordingsPath = RecordingsDirectoryManager.GetRecordingsDirectoryPath();
                if (Directory.Exists(recordingsPath))
                {
                    openFileDialog.InitialDirectory = recordingsPath;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    LaunchReplayForm(selectedFilePath);
                }
            }
        }

        private void LaunchReplayForm(string filePath)
        {
            try
            {
                ReplayForm replayForm = new ReplayForm(filePath);
                replayForm.Show();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The selected file could not be found.", "File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading replay: {ex.Message}", "Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog(this);
            }
        }
    }
}