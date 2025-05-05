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
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
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
            pnlLink.Size = new Size(782, 60);
            pnlLink.TabIndex = 1;
            // 
            // linkButton
            // 
            linkButton.BackColor = SystemColors.Window;
            linkButton.Font = new Font("Segoe UI", 10F);
            linkButton.ForeColor = SystemColors.ControlText;
            linkButton.Location = new Point(452, 14);
            linkButton.Name = "linkButton";
            linkButton.Size = new Size(149, 36);
            linkButton.TabIndex = 0;
            linkButton.Text = "Link Last.fm";
            linkButton.UseVisualStyleBackColor = false;
            linkButton.Click += linkButton_Click;
            // 
            // lastFmTextBox
            // 
            lastFmTextBox.BackColor = SystemColors.Window;
            lastFmTextBox.Font = new Font("Segoe UI", 10F);
            lastFmTextBox.ForeColor = SystemColors.WindowText;
            lastFmTextBox.Location = new Point(10, 15);
            lastFmTextBox.Name = "lastFmTextBox";
            lastFmTextBox.Size = new Size(427, 34);
            lastFmTextBox.TabIndex = 1;
            // 
            // lastFmDataGridView
            // 
            lastFmDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle17.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle17.ForeColor = Color.White;
            dataGridViewCellStyle17.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = DataGridViewTriState.True;
            lastFmDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            lastFmDataGridView.ColumnHeadersHeight = 34;
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle18.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle18.ForeColor = Color.White;
            dataGridViewCellStyle18.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.False;
            lastFmDataGridView.DefaultCellStyle = dataGridViewCellStyle18;
            lastFmDataGridView.Dock = DockStyle.Fill;
            lastFmDataGridView.EnableHeadersVisualStyles = false;
            lastFmDataGridView.GridColor = Color.Gray;
            lastFmDataGridView.Location = new Point(0, 60);
            lastFmDataGridView.Name = "lastFmDataGridView";
            lastFmDataGridView.RowHeadersWidth = 62;
            lastFmDataGridView.Size = new Size(782, 436);
            lastFmDataGridView.TabIndex = 0;
            // 
            // LinkLastFmForm
            // 
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(782, 496);
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
