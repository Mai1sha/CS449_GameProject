using SOSGame.Models;

namespace SOSGame
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadCurrentApiKeys();
        }

        private void LoadCurrentApiKeys()
        {
            string? openAIKey = OpenAIConfiguration.GetApiKey();
            if (!string.IsNullOrWhiteSpace(openAIKey))
            {
                txtOpenAIApiKey.Text = openAIKey;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string openAIApiKey = txtOpenAIApiKey.Text.Trim();

            bool hasOpenAIKey = !string.IsNullOrWhiteSpace(openAIApiKey);

            if (hasOpenAIKey && !ValidateOpenAIApiKeyFormat(openAIApiKey))
            {
                MessageBox.Show("Invalid OpenAI API key format. API key should start with 'sk-' and be at least 20 characters long.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (hasOpenAIKey)
                {
                    OpenAIConfiguration.SaveApiKeyToUserSettings(openAIApiKey);
                    MessageBox.Show("API key saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No API key to save. Please enter an API key.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving API key: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateOpenAIApiKeyFormat(string apiKey)
        {
            return apiKey.StartsWith("sk-") && apiKey.Length >= 20;
        }

        private void lnkGetOpenAIApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://platform.openai.com/api-keys",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Could not open browser. Please visit: https://platform.openai.com/api-keys",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
