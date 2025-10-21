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
            SOSGameLabel.Click += label1_Click;
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
            BoardSizeLabel.Click += label2_Click;
            // 
            // BoardSizeTextBox
            // 
            BoardSizeTextBox.Location = new Point(227, 123);
            BoardSizeTextBox.Name = "BoardSizeTextBox";
            BoardSizeTextBox.Size = new Size(125, 27);
            BoardSizeTextBox.TabIndex = 2;
            BoardSizeTextBox.TextChanged += BoardSizeTextBox_TextChanged;
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
            GameTypeLabel.Click += label3_Click;
            // 
            // startGameButton
            // 
            startGameButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            startGameButton.Location = new Point(248, 306);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(99, 43);
            startGameButton.TabIndex = 6;
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
            generalGameButton.CheckedChanged += generalGameButton_CheckedChanged;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 553);
            Controls.Add(generalGameButton);
            Controls.Add(simpleGameButton);
            Controls.Add(startGameButton);
            Controls.Add(GameTypeLabel);
            Controls.Add(BoardSizeTextBox);
            Controls.Add(BoardSizeLabel);
            Controls.Add(SOSGameLabel);
            Name = "StartForm";
            Text = "SOS Game";
            Load += Form1_Load;
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
    }
}
