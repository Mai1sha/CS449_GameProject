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
            panelGameBoard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelGameBoard.AutoScroll = true;
            panelGameBoard.BorderStyle = BorderStyle.Fixed3D;
            panelGameBoard.Location = new Point(12, 100);
            panelGameBoard.Name = "panelGameBoard";
            panelGameBoard.Size = new Size(500, 400);
            panelGameBoard.TabIndex = 0;
            // 
            // btnPlaceS
            // 
            btnPlaceS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPlaceS.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlaceS.Location = new Point(530, 200);
            btnPlaceS.Name = "btnPlaceS";
            btnPlaceS.Size = new Size(120, 50);
            btnPlaceS.TabIndex = 1;
            btnPlaceS.Text = "Place S";
            btnPlaceS.UseVisualStyleBackColor = true;
            // 
            // btnPlaceO
            // 
            btnPlaceO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPlaceO.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlaceO.Location = new Point(530, 260);
            btnPlaceO.Name = "btnPlaceO";
            btnPlaceO.Size = new Size(120, 50);
            btnPlaceO.TabIndex = 2;
            btnPlaceO.Text = "Place O";
            btnPlaceO.UseVisualStyleBackColor = true;
            // 
            // btnNewGame
            // 
            btnNewGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNewGame.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnNewGame.Location = new Point(530, 330);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(120, 50);
            btnNewGame.TabIndex = 3;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            // 
            // lblTurn
            // 
            lblTurn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTurn.AutoSize = true;
            lblTurn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTurn.Location = new Point(530, 100);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(158, 23);
            lblTurn.TabIndex = 4;
            lblTurn.Text = "Current turn: Blue";
            // 
            // lblGameStatus
            // 
            lblGameStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGameStatus.AutoSize = true;
            lblGameStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGameStatus.Location = new Point(530, 15);
            lblGameStatus.Name = "lblGameStatus";
            lblGameStatus.Size = new Size(130, 25);
            lblGameStatus.TabIndex = 5;
            lblGameStatus.Text = "Game: Active";
            // 
            // lblBlueScore
            // 
            lblBlueScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBlueScore.AutoSize = true;
            lblBlueScore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBlueScore.ForeColor = Color.Blue;
            lblBlueScore.Location = new Point(530, 50);
            lblBlueScore.Name = "lblBlueScore";
            lblBlueScore.Size = new Size(65, 23);
            lblBlueScore.TabIndex = 6;
            lblBlueScore.Text = "Blue: 0";
            // 
            // lblRedScore
            // 
            lblRedScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblRedScore.AutoSize = true;
            lblRedScore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRedScore.ForeColor = Color.Red;
            lblRedScore.Location = new Point(620, 50);
            lblRedScore.Name = "lblRedScore";
            lblRedScore.Size = new Size(61, 23);
            lblRedScore.TabIndex = 7;
            lblRedScore.Text = "Red: 0";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 520);
            MinimumSize = new Size(700, 520);
            Controls.Add(lblRedScore);
            Controls.Add(lblBlueScore);
            Controls.Add(lblGameStatus);
            Controls.Add(lblTurn);
            Controls.Add(btnNewGame);
            Controls.Add(btnPlaceO);
            Controls.Add(btnPlaceS);
            Controls.Add(panelGameBoard);
            Name = "GameForm";
            Text = "SOS Game";
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