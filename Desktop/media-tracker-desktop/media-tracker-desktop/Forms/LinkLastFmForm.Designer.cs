namespace media_tracker_desktop.Forms
{
    partial class LinkLastFmForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlLink;
        private System.Windows.Forms.TextBox lastFmTextBox;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.DataGridView lastFmDataGridView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLink = new System.Windows.Forms.Panel();
            this.lastFmTextBox = new System.Windows.Forms.TextBox();
            this.linkButton = new System.Windows.Forms.Button();
            this.lastFmDataGridView = new System.Windows.Forms.DataGridView();

            this.pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastFmDataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlLink
            // 
            this.pnlLink.Controls.Add(this.linkButton);
            this.pnlLink.Controls.Add(this.lastFmTextBox);
            this.pnlLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLink.Height = 60;
            this.pnlLink.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            // 
            // lastFmTextBox
            // 
            this.lastFmTextBox.ForeColor = System.Drawing.Color.White;
            this.lastFmTextBox.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lastFmTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lastFmTextBox.Location = new System.Drawing.Point(10, 15);
            this.lastFmTextBox.Width = 250;

            // 
            // linkButton
            // 
            this.linkButton.Text = "Link Last.fm";
            this.linkButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkButton.ForeColor = System.Drawing.Color.White;
            this.linkButton.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.linkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.linkButton.Location = new System.Drawing.Point(270, 13);
            this.linkButton.Click += new System.EventHandler(this.linkButton_Click);

            // 
            // lastFmDataGridView
            // 
            this.lastFmDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lastFmDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.lastFmDataGridView.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lastFmDataGridView.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.lastFmDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lastFmDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.lastFmDataGridView.GridColor = System.Drawing.Color.Gray;
            this.lastFmDataGridView.EnableHeadersVisualStyles = false;

            // 
            // LinkLastFmForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Controls.Add(this.lastFmDataGridView);
            this.Controls.Add(this.pnlLink);
            this.Name = "LinkLastFmForm";
            this.Text = "Last.fm";

            this.pnlLink.ResumeLayout(false);
            this.pnlLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastFmDataGridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
