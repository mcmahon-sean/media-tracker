namespace media_tracker_desktop.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnLinkSteam;
        private System.Windows.Forms.Button btnLinkLastFM;
        private System.Windows.Forms.Button btnLinkTmdb;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            btnLinkSteam = new Button();
            label1 = new Label();
            btnLinkTmdb = new Button();
            btnLinkLastFM = new Button();
            btnHome = new Button();
            lblTitle = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(16, 16, 16);
            pnlSidebar.Controls.Add(btnLinkSteam);
            pnlSidebar.Controls.Add(label1);
            pnlSidebar.Controls.Add(btnLinkTmdb);
            pnlSidebar.Controls.Add(btnLinkLastFM);
            pnlSidebar.Controls.Add(btnHome);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Margin = new Padding(2, 2, 2, 2);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(140, 390);
            pnlSidebar.TabIndex = 2;
            // 
            // btnLinkSteam
            // 
            btnLinkSteam.BackColor = Color.FromArgb(34, 37, 41);
            btnLinkSteam.FlatAppearance.BorderSize = 0;
            btnLinkSteam.FlatStyle = FlatStyle.Popup;
            btnLinkSteam.ForeColor = Color.White;
            btnLinkSteam.Location = new Point(8, 97);
            btnLinkSteam.Margin = new Padding(2, 2, 2, 2);
            btnLinkSteam.Name = "btnLinkSteam";
            btnLinkSteam.Size = new Size(119, 32);
            btnLinkSteam.TabIndex = 2;
            btnLinkSteam.Text = "Steam";
            btnLinkSteam.TextAlign = ContentAlignment.MiddleRight;
            btnLinkSteam.UseVisualStyleBackColor = false;
            btnLinkSteam.Click += btnLinkSteam_Click;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(32, 32, 32);
            label1.Location = new Point(10, 45);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(117, 1);
            label1.TabIndex = 4;
            // 
            // btnLinkTmdb
            // 
            btnLinkTmdb.BackColor = Color.FromArgb(34, 37, 41);
            btnLinkTmdb.FlatAppearance.BorderSize = 0;
            btnLinkTmdb.FlatStyle = FlatStyle.Popup;
            btnLinkTmdb.ForeColor = Color.White;
            btnLinkTmdb.Location = new Point(8, 140);
            btnLinkTmdb.Margin = new Padding(2, 2, 2, 2);
            btnLinkTmdb.Name = "btnLinkTmdb";
            btnLinkTmdb.Size = new Size(119, 32);
            btnLinkTmdb.TabIndex = 3;
            btnLinkTmdb.Text = "TMDB";
            btnLinkTmdb.TextAlign = ContentAlignment.MiddleRight;
            btnLinkTmdb.UseVisualStyleBackColor = false;
            btnLinkTmdb.Click += btnLinkTmdb_Click;
            // 
            // btnLinkLastFM
            // 
            btnLinkLastFM.BackColor = Color.FromArgb(34, 37, 41);
            btnLinkLastFM.FlatAppearance.BorderSize = 0;
            btnLinkLastFM.FlatStyle = FlatStyle.Popup;
            btnLinkLastFM.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLinkLastFM.ForeColor = Color.White;
            btnLinkLastFM.ImageAlign = ContentAlignment.MiddleLeft;
            btnLinkLastFM.Location = new Point(8, 54);
            btnLinkLastFM.Margin = new Padding(2, 2, 2, 2);
            btnLinkLastFM.Name = "btnLinkLastFM";
            btnLinkLastFM.Size = new Size(119, 32);
            btnLinkLastFM.TabIndex = 1;
            btnLinkLastFM.Text = "Last.fm";
            btnLinkLastFM.TextAlign = ContentAlignment.MiddleRight;
            btnLinkLastFM.UseVisualStyleBackColor = false;
            btnLinkLastFM.Click += btnLinkLastFM_Click;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(34, 37, 41);
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Popup;
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(8, 8);
            btnHome.Margin = new Padding(2, 2, 2, 2);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(119, 32);
            btnHome.TabIndex = 0;
            btnHome.Text = "Home";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(147, 6);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(160, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Media Tracker";
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(32, 32, 32);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(140, 0);
            pnlContent.Margin = new Padding(2, 2, 2, 2);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(560, 390);
            pnlContent.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(700, 390);
            Controls.Add(pnlContent);
            Controls.Add(lblTitle);
            Controls.Add(pnlSidebar);
            Margin = new Padding(2, 2, 2, 2);
            Name = "MainForm";
            Text = "Media Tracker";
            pnlSidebar.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label label1;
    }
}
