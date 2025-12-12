namespace SOSGame
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblOpenAISection = new Label();
            lblOpenAIApiKey = new Label();
            txtOpenAIApiKey = new TextBox();
            lnkGetOpenAIApiKey = new LinkLabel();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(150, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(150, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "AI Settings";
            // 
            // lblOpenAISection
            // 
            lblOpenAISection.AutoSize = true;
            lblOpenAISection.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOpenAISection.Location = new Point(30, 70);
            lblOpenAISection.Name = "lblOpenAISection";
            lblOpenAISection.Size = new Size(190, 25);
            lblOpenAISection.TabIndex = 1;
            lblOpenAISection.Text = "OpenAI Configuration";
            // 
            // lblOpenAIApiKey
            // 
            lblOpenAIApiKey.AutoSize = true;
            lblOpenAIApiKey.Font = new Font("Segoe UI", 10F);
            lblOpenAIApiKey.Location = new Point(30, 105);
            lblOpenAIApiKey.Name = "lblOpenAIApiKey";
            lblOpenAIApiKey.Size = new Size(71, 23);
            lblOpenAIApiKey.TabIndex = 2;
            lblOpenAIApiKey.Text = "API Key:";
            // 
            // txtOpenAIApiKey
            // 
            txtOpenAIApiKey.Font = new Font("Segoe UI", 10F);
            txtOpenAIApiKey.Location = new Point(30, 135);
            txtOpenAIApiKey.Name = "txtOpenAIApiKey";
            txtOpenAIApiKey.Size = new Size(380, 30);
            txtOpenAIApiKey.TabIndex = 3;
            txtOpenAIApiKey.UseSystemPasswordChar = true;
            // 
            // lnkGetOpenAIApiKey
            // 
            lnkGetOpenAIApiKey.AutoSize = true;
            lnkGetOpenAIApiKey.Font = new Font("Segoe UI", 9F);
            lnkGetOpenAIApiKey.Location = new Point(30, 175);
            lnkGetOpenAIApiKey.Name = "lnkGetOpenAIApiKey";
            lnkGetOpenAIApiKey.Size = new Size(186, 20);
            lnkGetOpenAIApiKey.TabIndex = 4;
            lnkGetOpenAIApiKey.TabStop = true;
            lnkGetOpenAIApiKey.Text = "Get your API key from here";
            lnkGetOpenAIApiKey.LinkClicked += lnkGetOpenAIApiKey_LinkClicked;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.Location = new Point(100, 230);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 40);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Location = new Point(230, 230);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 300);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(lnkGetOpenAIApiKey);
            Controls.Add(txtOpenAIApiKey);
            Controls.Add(lblOpenAIApiKey);
            Controls.Add(lblOpenAISection);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblOpenAISection;
        private Label lblOpenAIApiKey;
        private TextBox txtOpenAIApiKey;
        private LinkLabel lnkGetOpenAIApiKey;
        private Button btnSave;
        private Button btnCancel;
    }
}
