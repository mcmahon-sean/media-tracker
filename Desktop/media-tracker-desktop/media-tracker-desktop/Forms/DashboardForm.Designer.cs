namespace media_tracker_desktop.Forms
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblWelcome;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.lblWelcome = new Label();

            this.SuspendLayout();
            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font     = new System.Drawing.Font("Segoe UI", 14F);
            this.lblWelcome.ForeColor= System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name     = "lblWelcome";
            this.lblWelcome.Size     = new System.Drawing.Size(200, 25);
            this.lblWelcome.Text     = "Welcome!";
            // DashboardForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.lblWelcome);
            this.Name = "DashboardForm";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
