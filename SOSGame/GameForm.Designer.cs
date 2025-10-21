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
            lblTurn.Location = new Point(306, 90);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(174, 25);
            lblTurn.TabIndex = 4;
            lblTurn.Text = "Current turn: Blue";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}