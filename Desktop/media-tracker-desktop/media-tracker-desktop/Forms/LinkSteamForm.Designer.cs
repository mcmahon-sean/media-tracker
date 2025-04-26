namespace media_tracker_desktop.Forms
{
    partial class LinkSteamForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlLink;
        private System.Windows.Forms.TextBox steamTextBox;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.DataGridView steamDataGridView;

        private void InitializeComponent()
        {
            this.pnlLink = new System.Windows.Forms.Panel();
            this.steamTextBox = new System.Windows.Forms.TextBox();
            this.linkButton = new System.Windows.Forms.Button();
            this.steamDataGridView = new System.Windows.Forms.DataGridView();
            this.pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.steamDataGridView)).BeginInit();
            this.SuspendLayout();
            
            // pnlLink
            this.pnlLink.Controls.Add(this.steamTextBox);
            this.pnlLink.Controls.Add(this.linkButton);
            this.pnlLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLink.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.pnlLink.Location = new System.Drawing.Point(0, 0);
            this.pnlLink.Name = "pnlLink";
            this.pnlLink.Size = new System.Drawing.Size(800, 50);

            // steamTextBox
            this.steamTextBox.Location = new System.Drawing.Point(12, 12);
            this.steamTextBox.Name = "steamTextBox";
            this.steamTextBox.Size = new System.Drawing.Size(300, 23);

            // linkButton
            this.linkButton.Location = new System.Drawing.Point(320, 12);
            this.linkButton.Name = "linkButton";
            this.linkButton.Size = new System.Drawing.Size(100, 23);
            this.linkButton.Text = "Link Steam";
            this.linkButton.UseVisualStyleBackColor = true;
            this.linkButton.Click += new System.EventHandler(this.linkButton_Click);

            // steamDataGridView
            this.steamDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.steamDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.steamDataGridView.Location = new System.Drawing.Point(0, 50);
            this.steamDataGridView.Name = "steamDataGridView";
            this.steamDataGridView.Size = new System.Drawing.Size(800, 400);
            this.steamDataGridView.TabIndex = 0;

            // LinkSteamForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.steamDataGridView);
            this.Controls.Add(this.pnlLink);
            this.Name = "LinkSteamForm";
            this.Text = "Link Steam Account";
            this.pnlLink.ResumeLayout(false);
            this.pnlLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.steamDataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
