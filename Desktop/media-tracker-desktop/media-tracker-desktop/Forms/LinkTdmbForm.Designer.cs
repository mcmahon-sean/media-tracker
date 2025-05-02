namespace media_tracker_desktop.Forms
{
    partial class LinkTmdbForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlLink;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.DataGridView tmdbDataGridView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlLink = new Panel();
            linkButton = new Button();
            tmdbDataGridView = new DataGridView();
            pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tmdbDataGridView).BeginInit();
            SuspendLayout();
            // 
            // pnlLink
            // 
            pnlLink.BackColor = Color.FromArgb(45, 45, 48);
            pnlLink.Controls.Add(linkButton);
            pnlLink.Dock = DockStyle.Top;
            pnlLink.Location = new Point(0, 0);
            pnlLink.Name = "pnlLink";
            pnlLink.Size = new Size(638, 60);
            pnlLink.TabIndex = 1;
            // 
            // linkButton
            // 
            linkButton.BackColor = Color.FromArgb(80, 80, 80);
            linkButton.FlatStyle = FlatStyle.Flat;
            linkButton.Font = new Font("Segoe UI", 10F);
            linkButton.ForeColor = Color.White;
            linkButton.Location = new Point(12, 12);
            linkButton.Name = "linkButton";
            linkButton.Size = new Size(160, 36);
            linkButton.TabIndex = 0;
            linkButton.Text = "Link TMDB";
            linkButton.UseVisualStyleBackColor = false;
            linkButton.Click += linkButton_Click;
            // 
            // tmdbDataGridView
            // 
            tmdbDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            tmdbDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            tmdbDataGridView.ColumnHeadersHeight = 34;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            tmdbDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            tmdbDataGridView.Dock = DockStyle.Fill;
            tmdbDataGridView.EnableHeadersVisualStyles = false;
            tmdbDataGridView.GridColor = Color.Gray;
            tmdbDataGridView.Location = new Point(0, 60);
            tmdbDataGridView.Name = "tmdbDataGridView";
            tmdbDataGridView.RowHeadersVisible = false;
            tmdbDataGridView.RowHeadersWidth = 62;
            tmdbDataGridView.Size = new Size(638, 184);
            tmdbDataGridView.TabIndex = 0;
            // 
            // LinkTmdbForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(638, 244);
            Controls.Add(tmdbDataGridView);
            Controls.Add(pnlLink);
            Name = "LinkTmdbForm";
            Text = "TMDB";
            pnlLink.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tmdbDataGridView).EndInit();
            ResumeLayout(false);
        }
    }
}
