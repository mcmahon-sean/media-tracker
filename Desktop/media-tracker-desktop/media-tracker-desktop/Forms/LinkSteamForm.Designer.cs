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
            pnlLink = new Panel();
            steamTextBox = new TextBox();
            linkButton = new Button();
            steamDataGridView = new DataGridView();
            pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)steamDataGridView).BeginInit();
            SuspendLayout();
            // 
            // pnlLink
            // 
            pnlLink.BackColor = Color.FromArgb(45, 45, 48);
            pnlLink.Controls.Add(steamTextBox);
            pnlLink.Controls.Add(linkButton);
            pnlLink.Dock = DockStyle.Top;
            pnlLink.Location = new Point(0, 0);
            pnlLink.Margin = new Padding(4, 5, 4, 5);
            pnlLink.Name = "pnlLink";
            pnlLink.Size = new Size(1143, 83);
            pnlLink.TabIndex = 1;
            // 
            // steamTextBox
            // 
            steamTextBox.Location = new Point(17, 20);
            steamTextBox.Margin = new Padding(4, 5, 4, 5);
            steamTextBox.Name = "steamTextBox";
            steamTextBox.Size = new Size(427, 31);
            steamTextBox.TabIndex = 0;
            // 
            // linkButton
            // 
            linkButton.Location = new Point(457, 20);
            linkButton.Margin = new Padding(4, 5, 4, 5);
            linkButton.Name = "linkButton";
            linkButton.Size = new Size(143, 38);
            linkButton.TabIndex = 1;
            linkButton.Text = "Link Steam";
            linkButton.UseVisualStyleBackColor = true;
            linkButton.Click += linkButton_Click;
            // 
            // steamDataGridView
            // 
            steamDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            steamDataGridView.ColumnHeadersHeight = 34;
            steamDataGridView.Dock = DockStyle.Fill;
            steamDataGridView.Location = new Point(0, 83);
            steamDataGridView.Margin = new Padding(4, 5, 4, 5);
            steamDataGridView.Name = "steamDataGridView";
            steamDataGridView.RowHeadersWidth = 62;
            steamDataGridView.Size = new Size(1143, 667);
            steamDataGridView.TabIndex = 0;
            // 
            // LinkSteamForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1143, 750);
            Controls.Add(steamDataGridView);
            Controls.Add(pnlLink);
            Margin = new Padding(4, 5, 4, 5);
            Name = "LinkSteamForm";
            Text = "Link Steam Account";
            Load += LinkSteamForm_Load;
            pnlLink.ResumeLayout(false);
            pnlLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)steamDataGridView).EndInit();
            ResumeLayout(false);
        }
    }
}
