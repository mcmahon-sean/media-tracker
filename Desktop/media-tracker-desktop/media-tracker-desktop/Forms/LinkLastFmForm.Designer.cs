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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
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
            pnlLink.BackColor = Color.FromArgb(35, 35, 35);
            pnlLink.Controls.Add(linkButton);
            pnlLink.Controls.Add(lastFmTextBox);
            pnlLink.Dock = DockStyle.Top;
            pnlLink.Location = new Point(0, 0);
            pnlLink.Name = "pnlLink";
            pnlLink.Size = new Size(782, 83);
            pnlLink.TabIndex = 1;
            // 
            // linkButton
            // 
            linkButton.BackColor = SystemColors.Window;
            linkButton.Font = new Font("Segoe UI", 10F);
            linkButton.ForeColor = SystemColors.ControlText;
            linkButton.Location = new Point(457, 20);
            linkButton.Name = "linkButton";
            linkButton.Size = new Size(143, 38);
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
            lastFmTextBox.Location = new Point(22, 24);
            lastFmTextBox.Name = "lastFmTextBox";
            lastFmTextBox.Size = new Size(427, 34);
            lastFmTextBox.TabIndex = 1;
            // 
            // lastFmDataGridView
            // 
            lastFmDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            lastFmDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            lastFmDataGridView.ColumnHeadersHeight = 34;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            lastFmDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            lastFmDataGridView.Dock = DockStyle.Fill;
            lastFmDataGridView.EnableHeadersVisualStyles = false;
            lastFmDataGridView.GridColor = Color.Gray;
            lastFmDataGridView.Location = new Point(0, 83);
            lastFmDataGridView.Name = "lastFmDataGridView";
            lastFmDataGridView.RowHeadersWidth = 62;
            lastFmDataGridView.Size = new Size(782, 413);
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
