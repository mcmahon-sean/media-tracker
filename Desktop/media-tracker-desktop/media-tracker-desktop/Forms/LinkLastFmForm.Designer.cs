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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlLink = new Panel();
            linkButton = new Button();
            lastFmTextBox = new TextBox();
            lastFmDataGridView = new DataGridView();
            pnlLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lastFmDataGridView).BeginInit();
            SuspendLayout();
            // 
            // pnlLink
            // 
            pnlLink.BackColor = Color.FromArgb(45, 45, 48);
            pnlLink.Controls.Add(linkButton);
            pnlLink.Controls.Add(lastFmTextBox);
            pnlLink.Dock = DockStyle.Top;
            pnlLink.Location = new Point(0, 0);
            pnlLink.Name = "pnlLink";
            pnlLink.Size = new Size(669, 60);
            pnlLink.TabIndex = 1;
            // 
            // linkButton
            // 
            linkButton.BackColor = Color.FromArgb(80, 80, 80);
            linkButton.FlatStyle = FlatStyle.Flat;
            linkButton.Font = new Font("Segoe UI", 10F);
            linkButton.ForeColor = Color.White;
            linkButton.Location = new Point(270, 13);
            linkButton.Name = "linkButton";
            linkButton.Size = new Size(149, 36);
            linkButton.TabIndex = 0;
            linkButton.Text = "Link Last.fm";
            linkButton.UseVisualStyleBackColor = false;
            linkButton.Click += linkButton_Click;
            // 
            // lastFmTextBox
            // 
            lastFmTextBox.BackColor = Color.FromArgb(64, 64, 64);
            lastFmTextBox.Font = new Font("Segoe UI", 10F);
            lastFmTextBox.ForeColor = Color.White;
            lastFmTextBox.Location = new Point(10, 15);
            lastFmTextBox.Name = "lastFmTextBox";
            lastFmTextBox.Size = new Size(250, 34);
            lastFmTextBox.TabIndex = 1;
            // 
            // lastFmDataGridView
            // 
            lastFmDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            lastFmDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            lastFmDataGridView.ColumnHeadersHeight = 34;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            lastFmDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            lastFmDataGridView.Dock = DockStyle.Fill;
            lastFmDataGridView.EnableHeadersVisualStyles = false;
            lastFmDataGridView.GridColor = Color.Gray;
            lastFmDataGridView.Location = new Point(0, 60);
            lastFmDataGridView.Name = "lastFmDataGridView";
            lastFmDataGridView.RowHeadersWidth = 62;
            lastFmDataGridView.Size = new Size(669, 184);
            lastFmDataGridView.TabIndex = 0;
            // 
            // LinkLastFmForm
            // 
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(669, 244);
            Controls.Add(lastFmDataGridView);
            Controls.Add(pnlLink);
            Name = "LinkLastFmForm";
            Text = "Last.fm";
            pnlLink.ResumeLayout(false);
            pnlLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lastFmDataGridView).EndInit();
            ResumeLayout(false);
        }
    }
}
