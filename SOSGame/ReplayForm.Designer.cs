namespace SOSGame
{
    partial class ReplayForm
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
            btnNext = new Button();
            btnPrevious = new Button();
            btnReset = new Button();
            lblMoveCounter = new Label();
            lblGameInfo = new Label();
            lblBoardSize = new Label();
            lblGameMode = new Label();
            lblPlayerTypes = new Label();
            lblCurrentMove = new Label();
            lblFinalOutcome = new Label();
            SuspendLayout();
            // 
            // panelGameBoard
            // 
            panelGameBoard.BorderStyle = BorderStyle.Fixed3D;
            panelGameBoard.Location = new Point(15, 15);
            panelGameBoard.Name = "panelGameBoard";
            panelGameBoard.Size = new Size(500, 500);
            panelGameBoard.TabIndex = 0;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNext.Location = new Point(170, 530);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(120, 40);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next >";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPrevious.Location = new Point(40, 530);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(120, 40);
            btnPrevious.TabIndex = 2;
            btnPrevious.Text = "< Previous";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReset.Location = new Point(300, 530);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(120, 40);
            btnReset.TabIndex = 3;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            // 
            // lblMoveCounter
            // 
            lblMoveCounter.AutoSize = true;
            lblMoveCounter.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMoveCounter.Location = new Point(530, 30);
            lblMoveCounter.Name = "lblMoveCounter";
            lblMoveCounter.Size = new Size(120, 25);
            lblMoveCounter.TabIndex = 4;
            lblMoveCounter.Text = "Move: 0 / 0";
            // 
            // lblGameInfo
            // 
            lblGameInfo.AutoSize = true;
            lblGameInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblGameInfo.Location = new Point(530, 70);
            lblGameInfo.Name = "lblGameInfo";
            lblGameInfo.Size = new Size(115, 28);
            lblGameInfo.TabIndex = 5;
            lblGameInfo.Text = "Game Info";
            // 
            // lblBoardSize
            // 
            lblBoardSize.AutoSize = true;
            lblBoardSize.Font = new Font("Segoe UI", 10F);
            lblBoardSize.Location = new Point(530, 110);
            lblBoardSize.Name = "lblBoardSize";
            lblBoardSize.Size = new Size(100, 23);
            lblBoardSize.TabIndex = 6;
            lblBoardSize.Text = "Board: 0x0";
            // 
            // lblGameMode
            // 
            lblGameMode.AutoSize = true;
            lblGameMode.Font = new Font("Segoe UI", 10F);
            lblGameMode.Location = new Point(530, 140);
            lblGameMode.Name = "lblGameMode";
            lblGameMode.Size = new Size(63, 23);
            lblGameMode.TabIndex = 7;
            lblGameMode.Text = "Mode:";
            // 
            // lblPlayerTypes
            // 
            lblPlayerTypes.AutoSize = true;
            lblPlayerTypes.Font = new Font("Segoe UI", 10F);
            lblPlayerTypes.Location = new Point(530, 170);
            lblPlayerTypes.Name = "lblPlayerTypes";
            lblPlayerTypes.Size = new Size(70, 23);
            lblPlayerTypes.TabIndex = 8;
            lblPlayerTypes.Text = "Players:";
            // 
            // lblCurrentMove
            // 
            lblCurrentMove.AutoSize = true;
            lblCurrentMove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentMove.Location = new Point(530, 220);
            lblCurrentMove.Name = "lblCurrentMove";
            lblCurrentMove.Size = new Size(125, 23);
            lblCurrentMove.TabIndex = 9;
            lblCurrentMove.Text = "Current Move";
            // 
            // lblFinalOutcome
            // 
            lblFinalOutcome.AutoSize = true;
            lblFinalOutcome.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblFinalOutcome.Location = new Point(530, 280);
            lblFinalOutcome.Name = "lblFinalOutcome";
            lblFinalOutcome.Size = new Size(0, 25);
            lblFinalOutcome.TabIndex = 10;
            // 
            // ReplayForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Controls.Add(lblFinalOutcome);
            Controls.Add(lblCurrentMove);
            Controls.Add(lblPlayerTypes);
            Controls.Add(lblGameMode);
            Controls.Add(lblBoardSize);
            Controls.Add(lblGameInfo);
            Controls.Add(lblMoveCounter);
            Controls.Add(btnReset);
            Controls.Add(btnPrevious);
            Controls.Add(btnNext);
            Controls.Add(panelGameBoard);
            Name = "ReplayForm";
            Text = "Game Replay";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelGameBoard;
        private Button btnNext;
        private Button btnPrevious;
        private Button btnReset;
        private Label lblMoveCounter;
        private Label lblGameInfo;
        private Label lblBoardSize;
        private Label lblGameMode;
        private Label lblPlayerTypes;
        private Label lblCurrentMove;
        private Label lblFinalOutcome;
    }
}
