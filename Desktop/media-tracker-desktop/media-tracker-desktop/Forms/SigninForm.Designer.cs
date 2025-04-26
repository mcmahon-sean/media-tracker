namespace media_tracker_desktop.Forms
{
    partial class SigninForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panel1;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnSignIn;
        private Button btnRegister;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.BackColor   = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panel1      = new Panel();
            this.lblUsername = new Label();
            this.txtUsername = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.btnSignIn   = new Button();
            this.btnRegister = new Button();

            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // panel1
            this.panel1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.panel1.Controls.Add(this.lblUsername);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.lblPassword);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.btnSignIn);
            this.panel1.Controls.Add(this.btnRegister);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Size     = new System.Drawing.Size(360, 180);
            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.ForeColor = System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(20, 20);
            this.lblUsername.Text     = "Username:";
            // txtUsername
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location  = new System.Drawing.Point(100, 17);
            this.txtUsername.Size      = new System.Drawing.Size(240, 23);
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.ForeColor= System.Drawing.Color.White;
            this.lblPassword.Location = new System.Drawing.Point(20, 60);
            this.lblPassword.Text     = "Password:";
            // txtPassword
            this.txtPassword.BackColor    = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtPassword.ForeColor    = System.Drawing.Color.White;
            this.txtPassword.Location     = new System.Drawing.Point(100, 57);
            this.txtPassword.Size         = new System.Drawing.Size(240, 23);
            this.txtPassword.PasswordChar = '*';
            // btnSignIn
            this.btnSignIn.BackColor            = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnSignIn.FlatAppearance.BorderSize = 0;
            this.btnSignIn.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignIn.ForeColor            = System.Drawing.Color.White;
            this.btnSignIn.Location             = new System.Drawing.Point(100, 100);
            this.btnSignIn.Size                 = new System.Drawing.Size(100, 30);
            this.btnSignIn.Text                 = "Sign In";
            this.btnSignIn.UseVisualStyleBackColor = false;
            this.btnSignIn.Click               += new System.EventHandler(this.btnSignIn_Click);
            // btnRegister
            this.btnRegister.BackColor            = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle            = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.ForeColor            = System.Drawing.Color.White;
            this.btnRegister.Location             = new System.Drawing.Point(210, 100);
            this.btnRegister.Size                 = new System.Drawing.Size(100, 30);
            this.btnRegister.Text                 = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click               += new System.EventHandler(this.btnRegister_Click);
            // SigninForm
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.panel1);
            this.Name    = "SigninForm";
            this.Text    = "Sign In";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
