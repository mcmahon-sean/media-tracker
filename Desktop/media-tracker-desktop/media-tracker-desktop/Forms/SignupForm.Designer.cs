namespace media_tracker_desktop.Forms
{
    partial class SignupForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panel1, panel2;
        private Label SignUpLabel, lblUsername, lblFN, lblLN, lblEmail, PWLabel;
        private TextBox txtUsername, txtFN, txtLN, txtEmail, txtPassword;
        private Button btnRegister;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.BackColor    = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panel2       = new Panel();
            this.SignUpLabel  = new Label();
            this.panel1       = new Panel();
            this.lblUsername  = new Label();
            this.txtUsername  = new TextBox();
            this.lblFN        = new Label();
            this.txtFN        = new TextBox();
            this.lblLN        = new Label();
            this.txtLN        = new TextBox();
            this.lblEmail     = new Label();
            this.txtEmail     = new TextBox();
            this.PWLabel      = new Label();
            this.txtPassword  = new TextBox();
            this.btnRegister  = new Button();

            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // panel2
            this.panel2.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.panel2.Controls.Add(this.SignUpLabel);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Size     = new System.Drawing.Size(360, 60);
            // SignUpLabel
            this.SignUpLabel.AutoSize  = true;
            this.SignUpLabel.Font      = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.SignUpLabel.ForeColor = System.Drawing.Color.White;
            this.SignUpLabel.Location  = new System.Drawing.Point(130, 15);
            this.SignUpLabel.Text      = "Sign Up";
            // panel1
            this.panel1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblUsername);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.lblFN);
            this.panel1.Controls.Add(this.txtFN);
            this.panel1.Controls.Add(this.lblLN);
            this.panel1.Controls.Add(this.txtLN);
            this.panel1.Controls.Add(this.lblEmail);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.PWLabel);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.btnRegister);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Size     = new System.Drawing.Size(360, 350);
            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.ForeColor= System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(20, 90);
            this.lblUsername.Text     = "Username:";
            // txtUsername
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location  = new System.Drawing.Point(120, 87);
            this.txtUsername.Size      = new System.Drawing.Size(240, 23);
            // lblFN
            this.lblFN.AutoSize    = true;
            this.lblFN.ForeColor   = System.Drawing.Color.White;
            this.lblFN.Location    = new System.Drawing.Point(20, 130);
            this.lblFN.Text        = "First Name:";
            // txtFN
            this.txtFN.BackColor   = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtFN.ForeColor   = System.Drawing.Color.White;
            this.txtFN.Location    = new System.Drawing.Point(120, 127);
            this.txtFN.Size        = new System.Drawing.Size(240, 23);
            // lblLN
            this.lblLN.AutoSize    = true;
            this.lblLN.ForeColor   = System.Drawing.Color.White;
            this.lblLN.Location    = new System.Drawing.Point(20, 170);
            this.lblLN.Text        = "Last Name:";
            // txtLN
            this.txtLN.BackColor   = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtLN.ForeColor   = System.Drawing.Color.White;
            this.txtLN.Location    = new System.Drawing.Point(120, 167);
            this.txtLN.Size        = new System.Drawing.Size(240, 23);
            // lblEmail
            this.lblEmail.AutoSize  = true;
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location  = new System.Drawing.Point(20, 210);
            this.lblEmail.Text      = "Email:";
            // txtEmail
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtEmail.ForeColor = System.Drawing.Color.White;
            this.txtEmail.Location  = new System.Drawing.Point(120, 207);
            this.txtEmail.Size      = new System.Drawing.Size(240, 23);
            // PWLabel
            this.PWLabel.AutoSize   = true;
            this.PWLabel.ForeColor  = System.Drawing.Color.White;
            this.PWLabel.Location   = new System.Drawing.Point(20, 250);
            this.PWLabel.Text       = "Password:";
            // txtPassword
            this.txtPassword.BackColor    = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtPassword.ForeColor    = System.Drawing.Color.White;
            this.txtPassword.Location     = new System.Drawing.Point(120, 247);
            this.txtPassword.Size         = new System.Drawing.Size(240, 23);
            this.txtPassword.PasswordChar = '*';
            // btnRegister
            this.btnRegister.BackColor            = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.ForeColor            = System.Drawing.Color.White;
            this.btnRegister.Location             = new System.Drawing.Point(130, 290);
            this.btnRegister.Size                 = new System.Drawing.Size(100, 30);
            this.btnRegister.Text                 = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click               += new System.EventHandler(this.btnRegister_Click);
            // SignupForm
            this.ClientSize = new System.Drawing.Size(384, 401);
            this.Controls.Add(this.panel1);
            this.Name    = "SignupForm";
            this.Text    = "Sign Up";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
