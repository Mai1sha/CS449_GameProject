namespace SOSGame
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelGameBoard = new Panel();
            btnPlaceS = new Button();
            btnPlaceO = new Button();
            btnNewGame = new Button();
            lblTurn = new Label();
            lblGameStatus = new Label();
            lblBlueScore = new Label();
            lblRedScore = new Label();
            SuspendLayout();
            // 
            // panelGameBoard
            // 
            panelGameBoard.AutoScroll = true;
            panelGameBoard.BorderStyle = BorderStyle.Fixed3D;
            panelGameBoard.Location = new Point(293, 141);
            panelGameBoard.Name = "panelGameBoard";
            panelGameBoard.Size = new Size(200, 100);
            panelGameBoard.TabIndex = 0;
            // 
            // btnPlaceS
            // 
            btnPlaceS.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlaceS.Location = new Point(599, 311);
            btnPlaceS.Name = "btnPlaceS";
            btnPlaceS.Size = new Size(100, 60);
            btnPlaceS.TabIndex = 1;
            btnPlaceS.Text = "Place S";
            btnPlaceS.UseVisualStyleBackColor = true;
            // 
            // btnPlaceO
            // 
            btnPlaceO.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlaceO.Location = new Point(393, 311);
            btnPlaceO.Name = "btnPlaceO";
            btnPlaceO.Size = new Size(100, 60);
            btnPlaceO.TabIndex = 2;
            btnPlaceO.Text = "Place O";
            btnPlaceO.UseVisualStyleBackColor = true;
            // 
            // btnNewGame
            // 
            btnNewGame.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnNewGame.Location = new Point(147, 319);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(163, 40);
            btnNewGame.TabIndex = 3;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            // 
            // lblTurn
            // 
            lblTurn.AutoSize = true;
            lblTurn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTurn.Location = new Point(576, 94);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(174, 25);
            lblTurn.TabIndex = 4;
            lblTurn.Text = "Current turn: Blue";
            // 
            // lblGameStatus
            // 
            lblGameStatus.AutoSize = true;
            lblGameStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblGameStatus.Location = new Point(576, 49);
            lblGameStatus.Name = "lblGameStatus";
            lblGameStatus.Size = new Size(137, 28);
            lblGameStatus.TabIndex = 5;
            lblGameStatus.Text = "Game: Active";
            // 
            // lblBlueScore
            // 
            lblBlueScore.AutoSize = true;
            lblBlueScore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBlueScore.ForeColor = Color.Blue;
            lblBlueScore.Location = new Point(576, 141);
            lblBlueScore.Name = "lblBlueScore";
            lblBlueScore.Size = new Size(65, 23);
            lblBlueScore.TabIndex = 6;
            lblBlueScore.Text = "Blue: 0";
            lblBlueScore.Click += lblBlueScore_Click;
            // 
            // lblRedScore
            // 
            lblRedScore.AutoSize = true;
            lblRedScore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRedScore.ForeColor = Color.Red;
            lblRedScore.Location = new Point(576, 173);
            lblRedScore.Name = "lblRedScore";
            lblRedScore.Size = new Size(61, 23);
            lblRedScore.TabIndex = 7;
            lblRedScore.Text = "Red: 0";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblRedScore);
            Controls.Add(lblBlueScore);
            Controls.Add(lblGameStatus);
            Controls.Add(lblTurn);
            Controls.Add(btnNewGame);
            Controls.Add(btnPlaceO);
            Controls.Add(btnPlaceS);
            Controls.Add(panelGameBoard);
            Name = "GameForm";
            Text = "GameForm";
            Load += GameForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelGameBoard;
        private System.Windows.Forms.Button btnPlaceS;
        private System.Windows.Forms.Button btnPlaceO;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Label lblTurn;
        private System.Windows.Forms.Label lblGameStatus;
        private System.Windows.Forms.Label lblBlueScore;
        private System.Windows.Forms.Label lblRedScore;
    }
}