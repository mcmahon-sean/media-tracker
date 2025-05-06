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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
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
            pnlLink.BackColor = Color.FromArgb(35, 35, 35);
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
            steamTextBox.Location = new Point(22, 24);
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
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            steamDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            steamDataGridView.ColumnHeadersHeight = 34;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = Color.White;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            steamDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            steamDataGridView.Dock = DockStyle.Fill;
            steamDataGridView.EnableHeadersVisualStyles = false;
            steamDataGridView.GridColor = Color.Gray;
            steamDataGridView.Location = new Point(0, 83);
            steamDataGridView.Margin = new Padding(4, 5, 4, 5);
            steamDataGridView.Name = "steamDataGridView";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            steamDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            steamDataGridView.RowHeadersVisible = false;
            steamDataGridView.RowHeadersWidth = 62;
            steamDataGridView.Size = new Size(1143, 667);
            steamDataGridView.TabIndex = 0;
            // 
            // LinkSteamForm
            // 
            AcceptButton = linkButton;
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
