namespace media_tracker_desktop.Forms
{
    partial class LinkTmdbForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlLink;
        private System.Windows.Forms.TextBox tmdbIdTextBox;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.DataGridView tmdbDataGridView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLink = new System.Windows.Forms.Panel();
            this.tmdbIdTextBox = new System.Windows.Forms.TextBox();
            this.linkButton = new System.Windows.Forms.Button();
            this.tmdbDataGridView = new System.Windows.Forms.DataGridView();

            this.pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmdbDataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlLink
            // 
            this.pnlLink.Controls.Add(this.linkButton);
            this.pnlLink.Controls.Add(this.tmdbIdTextBox);
            this.pnlLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLink.Height = 60;
            this.pnlLink.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            // 
            // tmdbIdTextBox
            // 
            this.tmdbIdTextBox.ForeColor = System.Drawing.Color.White;
            this.tmdbIdTextBox.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.tmdbIdTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tmdbIdTextBox.Location = new System.Drawing.Point(10, 15);
            this.tmdbIdTextBox.Width = 250;

            // 
            // linkButton
            // 
            this.linkButton.Text = "Link TMDB ID";
            this.linkButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkButton.ForeColor = System.Drawing.Color.White;
            this.linkButton.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.linkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.linkButton.Location = new System.Drawing.Point(270, 13);
            this.linkButton.Click += new System.EventHandler(this.linkButton_Click);

            // 
            // tmdbDataGridView
            // 
            this.tmdbDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmdbDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.tmdbDataGridView.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.tmdbDataGridView.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.tmdbDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.tmdbDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.tmdbDataGridView.GridColor = System.Drawing.Color.Gray;
            this.tmdbDataGridView.EnableHeadersVisualStyles = false;

            // 
            // LinkTmdbForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Controls.Add(this.tmdbDataGridView);
            this.Controls.Add(this.pnlLink);
            this.Name = "LinkTmdbForm";
            this.Text = "TMDB";

            this.pnlLink.ResumeLayout(false);
            this.pnlLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmdbDataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
