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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnLinkTmdb = new System.Windows.Forms.Button();
            this.btnLinkLastFM = new System.Windows.Forms.Button();
            this.btnLinkSteam = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();

            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();

            // MainForm
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "MainForm";
            this.Text = "Media Tracker";

            // pnlSidebar
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.pnlSidebar.Controls.Add(this.btnLinkTmdb);
            this.pnlSidebar.Controls.Add(this.btnLinkLastFM);
            this.pnlSidebar.Controls.Add(this.btnLinkSteam);
            this.pnlSidebar.Controls.Add(this.btnHome);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(200, 650);

            // btnHome
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Size = new System.Drawing.Size(200, 50);
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);

            // btnLinkSteam
            this.btnLinkSteam.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnLinkSteam.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLinkSteam.FlatAppearance.BorderSize = 0;
            this.btnLinkSteam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLinkSteam.ForeColor = System.Drawing.Color.White;
            this.btnLinkSteam.Size = new System.Drawing.Size(200, 50);
            this.btnLinkSteam.Text = "Steam";
            this.btnLinkSteam.UseVisualStyleBackColor = false;
            this.btnLinkSteam.Click += new System.EventHandler(this.btnLinkSteam_Click);

            // btnLinkLastFM
            this.btnLinkLastFM.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnLinkLastFM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLinkLastFM.FlatAppearance.BorderSize = 0;
            this.btnLinkLastFM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLinkLastFM.ForeColor = System.Drawing.Color.White;
            this.btnLinkLastFM.Size = new System.Drawing.Size(200, 50);
            this.btnLinkLastFM.Text = "Last.fm";
            this.btnLinkLastFM.UseVisualStyleBackColor = false;
            this.btnLinkLastFM.Click += new System.EventHandler(this.btnLinkLastFM_Click);

            // btnLinkTmdb
            this.btnLinkTmdb.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnLinkTmdb.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLinkTmdb.FlatAppearance.BorderSize = 0;
            this.btnLinkTmdb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLinkTmdb.ForeColor = System.Drawing.Color.White;
            this.btnLinkTmdb.Size = new System.Drawing.Size(200, 50);
            this.btnLinkTmdb.Text = "TMDB";
            this.btnLinkTmdb.UseVisualStyleBackColor = false;
            this.btnLinkTmdb.Click += new System.EventHandler(this.btnLinkTmdb_Click);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(210, 10);
            this.lblTitle.Text = "Media Tracker";

            // pnlContent
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(200, 50);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 600);

            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
 #endregion
    }
}
