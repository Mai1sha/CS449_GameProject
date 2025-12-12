namespace SOSGame
{
    partial class StartForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SOSGameLabel = new Label();
            BoardSizeLabel = new Label();
            BoardSizeTextBox = new TextBox();
            GameTypeLabel = new Label();
            startGameButton = new Button();
            simpleGameButton = new RadioButton();
            generalGameButton = new RadioButton();
            bluePlayerGroupBox = new GroupBox();
            bluePlayerLabel = new Label();
            blueHumanRadio = new RadioButton();
            blueComputerRadio = new RadioButton();
            blueOpenAIRadio = new RadioButton();
            redPlayerGroupBox = new GroupBox();
            redPlayerLabel = new Label();
            redHumanRadio = new RadioButton();
            redComputerRadio = new RadioButton();
            redOpenAIRadio = new RadioButton();
            chkRecordGame = new CheckBox();
            btnReplay = new Button();
            btnSettings = new Button();
            bluePlayerGroupBox.SuspendLayout();
            redPlayerGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // SOSGameLabel
            // 
            SOSGameLabel.AutoSize = true;
            SOSGameLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            SOSGameLabel.Location = new Point(231, 31);
            SOSGameLabel.Name = "SOSGameLabel";
            SOSGameLabel.Size = new Size(138, 32);
            SOSGameLabel.TabIndex = 0;
            SOSGameLabel.Text = "SOS Game!";
            // 
            // BoardSizeLabel
            // 
            BoardSizeLabel.AutoSize = true;
            BoardSizeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            BoardSizeLabel.Location = new Point(42, 119);
            BoardSizeLabel.Name = "BoardSizeLabel";
            BoardSizeLabel.Size = new Size(173, 28);
            BoardSizeLabel.TabIndex = 1;
            BoardSizeLabel.Text = "Enter Board Size:";
            // 
            // BoardSizeTextBox
            // 
            BoardSizeTextBox.Location = new Point(227, 123);
            BoardSizeTextBox.Name = "BoardSizeTextBox";
            BoardSizeTextBox.Size = new Size(125, 27);
            BoardSizeTextBox.TabIndex = 2;
            // 
            // GameTypeLabel
            // 
            GameTypeLabel.AutoSize = true;
            GameTypeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            GameTypeLabel.Location = new Point(42, 185);
            GameTypeLabel.Name = "GameTypeLabel";
            GameTypeLabel.Size = new Size(191, 28);
            GameTypeLabel.TabIndex = 3;
            GameTypeLabel.Text = "Select Game Type: ";
            // 
            // startGameButton
            // 
            startGameButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            startGameButton.Location = new Point(220, 440);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(140, 50);
            startGameButton.TabIndex = 11;
            startGameButton.Text = "Start Game";
            startGameButton.UseVisualStyleBackColor = true;
            startGameButton.Click += startGameButton_Click;
            // 
            // simpleGameButton
            // 
            simpleGameButton.AutoSize = true;
            simpleGameButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            simpleGameButton.Location = new Point(230, 190);
            simpleGameButton.Name = "simpleGameButton";
            simpleGameButton.Size = new Size(122, 24);
            simpleGameButton.TabIndex = 7;
            simpleGameButton.TabStop = true;
            simpleGameButton.Text = "Simple Game";
            simpleGameButton.UseVisualStyleBackColor = true;
            // 
            // generalGameButton
            // 
            generalGameButton.AutoSize = true;
            generalGameButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            generalGameButton.Location = new Point(353, 190);
            generalGameButton.Name = "generalGameButton";
            generalGameButton.Size = new Size(129, 24);
            generalGameButton.TabIndex = 8;
            generalGameButton.TabStop = true;
            generalGameButton.Text = "General Game";
            generalGameButton.UseVisualStyleBackColor = true;
            // 
            // bluePlayerGroupBox
            // 
            bluePlayerGroupBox.Controls.Add(blueOpenAIRadio);
            bluePlayerGroupBox.Controls.Add(blueComputerRadio);
            bluePlayerGroupBox.Controls.Add(blueHumanRadio);
            bluePlayerGroupBox.Controls.Add(bluePlayerLabel);
            bluePlayerGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            bluePlayerGroupBox.ForeColor = Color.Blue;
            bluePlayerGroupBox.Location = new Point(42, 240);
            bluePlayerGroupBox.Name = "bluePlayerGroupBox";
            bluePlayerGroupBox.Size = new Size(220, 160);
            bluePlayerGroupBox.TabIndex = 9;
            bluePlayerGroupBox.TabStop = false;
            bluePlayerGroupBox.Text = "Blue Player";
            // 
            // bluePlayerLabel
            // 
            bluePlayerLabel.AutoSize = true;
            bluePlayerLabel.Font = new Font("Segoe UI", 9F);
            bluePlayerLabel.ForeColor = Color.Black;
            bluePlayerLabel.Location = new Point(15, 30);
            bluePlayerLabel.Name = "bluePlayerLabel";
            bluePlayerLabel.Size = new Size(84, 20);
            bluePlayerLabel.TabIndex = 0;
            bluePlayerLabel.Text = "Select type:";
            // 
            // blueHumanRadio
            // 
            blueHumanRadio.AutoSize = true;
            blueHumanRadio.Checked = true;
            blueHumanRadio.Font = new Font("Segoe UI", 9F);
            blueHumanRadio.ForeColor = Color.Black;
            blueHumanRadio.Location = new Point(15, 55);
            blueHumanRadio.Name = "blueHumanRadio";
            blueHumanRadio.Size = new Size(76, 24);
            blueHumanRadio.TabIndex = 1;
            blueHumanRadio.TabStop = true;
            blueHumanRadio.Text = "Human";
            blueHumanRadio.UseVisualStyleBackColor = true;
            // 
            // blueComputerRadio
            // 
            blueComputerRadio.AutoSize = true;
            blueComputerRadio.Font = new Font("Segoe UI", 9F);
            blueComputerRadio.ForeColor = Color.Black;
            blueComputerRadio.Location = new Point(110, 55);
            blueComputerRadio.Name = "blueComputerRadio";
            blueComputerRadio.Size = new Size(96, 24);
            blueComputerRadio.TabIndex = 2;
            blueComputerRadio.Text = "Computer";
            blueComputerRadio.UseVisualStyleBackColor = true;
            // 
            // blueOpenAIRadio
            // 
            blueOpenAIRadio.AutoSize = true;
            blueOpenAIRadio.Font = new Font("Segoe UI", 9F);
            blueOpenAIRadio.ForeColor = Color.Black;
            blueOpenAIRadio.Location = new Point(15, 85);
            blueOpenAIRadio.Name = "blueOpenAIRadio";
            blueOpenAIRadio.Size = new Size(82, 24);
            blueOpenAIRadio.TabIndex = 3;
            blueOpenAIRadio.Text = "OpenAI";
            blueOpenAIRadio.UseVisualStyleBackColor = true;
            // 
            // redPlayerGroupBox
            // 
            redPlayerGroupBox.Controls.Add(redOpenAIRadio);
            redPlayerGroupBox.Controls.Add(redComputerRadio);
            redPlayerGroupBox.Controls.Add(redHumanRadio);
            redPlayerGroupBox.Controls.Add(redPlayerLabel);
            redPlayerGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            redPlayerGroupBox.ForeColor = Color.Red;
            redPlayerGroupBox.Location = new Point(290, 240);
            redPlayerGroupBox.Name = "redPlayerGroupBox";
            redPlayerGroupBox.Size = new Size(220, 160);
            redPlayerGroupBox.TabIndex = 10;
            redPlayerGroupBox.TabStop = false;
            redPlayerGroupBox.Text = "Red Player";
            // 
            // redPlayerLabel
            // 
            redPlayerLabel.AutoSize = true;
            redPlayerLabel.Font = new Font("Segoe UI", 9F);
            redPlayerLabel.ForeColor = Color.Black;
            redPlayerLabel.Location = new Point(15, 30);
            redPlayerLabel.Name = "redPlayerLabel";
            redPlayerLabel.Size = new Size(84, 20);
            redPlayerLabel.TabIndex = 0;
            redPlayerLabel.Text = "Select type:";
            // 
            // redHumanRadio
            // 
            redHumanRadio.AutoSize = true;
            redHumanRadio.Checked = true;
            redHumanRadio.Font = new Font("Segoe UI", 9F);
            redHumanRadio.ForeColor = Color.Black;
            redHumanRadio.Location = new Point(15, 55);
            redHumanRadio.Name = "redHumanRadio";
            redHumanRadio.Size = new Size(76, 24);
            redHumanRadio.TabIndex = 1;
            redHumanRadio.TabStop = true;
            redHumanRadio.Text = "Human";
            redHumanRadio.UseVisualStyleBackColor = true;
            // 
            // redComputerRadio
            // 
            redComputerRadio.AutoSize = true;
            redComputerRadio.Font = new Font("Segoe UI", 9F);
            redComputerRadio.ForeColor = Color.Black;
            redComputerRadio.Location = new Point(110, 55);
            redComputerRadio.Name = "redComputerRadio";
            redComputerRadio.Size = new Size(96, 24);
            redComputerRadio.TabIndex = 2;
            redComputerRadio.Text = "Computer";
            redComputerRadio.UseVisualStyleBackColor = true;
            // 
            // redOpenAIRadio
            // 
            redOpenAIRadio.AutoSize = true;
            redOpenAIRadio.Font = new Font("Segoe UI", 9F);
            redOpenAIRadio.ForeColor = Color.Black;
            redOpenAIRadio.Location = new Point(15, 85);
            redOpenAIRadio.Name = "redOpenAIRadio";
            redOpenAIRadio.Size = new Size(82, 24);
            redOpenAIRadio.TabIndex = 3;
            redOpenAIRadio.Text = "OpenAI";
            redOpenAIRadio.UseVisualStyleBackColor = true;
            // 
            // chkRecordGame
            // 
            chkRecordGame.AutoSize = true;
            chkRecordGame.Font = new Font("Segoe UI", 10F);
            chkRecordGame.Location = new Point(42, 420);
            chkRecordGame.Name = "chkRecordGame";
            chkRecordGame.Size = new Size(133, 27);
            chkRecordGame.TabIndex = 12;
            chkRecordGame.Text = "Record game";
            chkRecordGame.UseVisualStyleBackColor = true;
            // 
            // btnReplay
            // 
            btnReplay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReplay.Location = new Point(390, 440);
            btnReplay.Name = "btnReplay";
            btnReplay.Size = new Size(120, 50);
            btnReplay.TabIndex = 13;
            btnReplay.Text = "Replay";
            btnReplay.UseVisualStyleBackColor = true;
            btnReplay.Click += btnReplay_Click;
            // 
            // btnSettings
            // 
            btnSettings.Font = new Font("Segoe UI", 9F);
            btnSettings.Location = new Point(420, 31);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(90, 32);
            btnSettings.TabIndex = 14;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 530);
            MinimumSize = new Size(550, 530);
            Controls.Add(btnSettings);
            Controls.Add(btnReplay);
            Controls.Add(chkRecordGame);
            Controls.Add(redPlayerGroupBox);
            Controls.Add(bluePlayerGroupBox);
            Controls.Add(generalGameButton);
            Controls.Add(simpleGameButton);
            Controls.Add(startGameButton);
            Controls.Add(GameTypeLabel);
            Controls.Add(BoardSizeTextBox);
            Controls.Add(BoardSizeLabel);
            Controls.Add(SOSGameLabel);
            Name = "StartForm";
            Text = "SOS Game - Setup";
            bluePlayerGroupBox.ResumeLayout(false);
            bluePlayerGroupBox.PerformLayout();
            redPlayerGroupBox.ResumeLayout(false);
            redPlayerGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label SOSGameLabel;
        private Label BoardSizeLabel;
        private TextBox BoardSizeTextBox;
        private Label GameTypeLabel;
        private Button startGameButton;
        private RadioButton simpleGameButton;
        private RadioButton generalGameButton;
        private GroupBox bluePlayerGroupBox;
        private Label bluePlayerLabel;
        private RadioButton blueHumanRadio;
        private RadioButton blueComputerRadio;
        private RadioButton blueOpenAIRadio;
        private GroupBox redPlayerGroupBox;
        private Label redPlayerLabel;
        private RadioButton redHumanRadio;
        private RadioButton redComputerRadio;
        private RadioButton redOpenAIRadio;
        private CheckBox chkRecordGame;
        private Button btnReplay;
        private Button btnSettings;
    }
}
