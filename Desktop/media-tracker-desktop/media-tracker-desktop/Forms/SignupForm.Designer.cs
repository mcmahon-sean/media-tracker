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
            panel2 = new Panel();
            SignUpLabel = new Label();
            panel1 = new Panel();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblFN = new Label();
            txtFN = new TextBox();
            lblLN = new Label();
            txtLN = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            PWLabel = new Label();
            txtPassword = new TextBox();
            btnRegister = new Button();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(32, 32, 32);
            panel2.Controls.Add(SignUpLabel);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(360, 60);
            panel2.TabIndex = 0;
            // 
            // SignUpLabel
            // 
            SignUpLabel.AutoSize = true;
            SignUpLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            SignUpLabel.ForeColor = Color.White;
            SignUpLabel.Location = new Point(130, 15);
            SignUpLabel.Name = "SignUpLabel";
            SignUpLabel.Size = new Size(120, 38);
            SignUpLabel.TabIndex = 0;
            SignUpLabel.Text = "Sign Up";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 32, 32);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(lblUsername);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(lblFN);
            panel1.Controls.Add(txtFN);
            panel1.Controls.Add(lblLN);
            panel1.Controls.Add(txtLN);
            panel1.Controls.Add(lblEmail);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(PWLabel);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(btnRegister);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(385, 350);
            panel1.TabIndex = 0;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(20, 90);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(95, 25);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.White;
            txtUsername.ForeColor = Color.Black;
            txtUsername.Location = new Point(132, 87);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(240, 31);
            txtUsername.TabIndex = 2;
            // 
            // lblFN
            // 
            lblFN.AutoSize = true;
            lblFN.ForeColor = Color.White;
            lblFN.Location = new Point(20, 130);
            lblFN.Name = "lblFN";
            lblFN.Size = new Size(101, 25);
            lblFN.TabIndex = 3;
            lblFN.Text = "First Name:";
            // 
            // txtFN
            // 
            txtFN.BackColor = Color.White;
            txtFN.ForeColor = Color.Black;
            txtFN.Location = new Point(132, 127);
            txtFN.Name = "txtFN";
            txtFN.Size = new Size(240, 31);
            txtFN.TabIndex = 4;
            // 
            // lblLN
            // 
            lblLN.AutoSize = true;
            lblLN.ForeColor = Color.White;
            lblLN.Location = new Point(20, 170);
            lblLN.Name = "lblLN";
            lblLN.Size = new Size(99, 25);
            lblLN.TabIndex = 5;
            lblLN.Text = "Last Name:";
            // 
            // txtLN
            // 
            txtLN.BackColor = Color.White;
            txtLN.ForeColor = Color.Black;
            txtLN.Location = new Point(132, 167);
            txtLN.Name = "txtLN";
            txtLN.Size = new Size(240, 31);
            txtLN.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = Color.White;
            lblEmail.Location = new Point(20, 210);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(58, 25);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(132, 207);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(240, 31);
            txtEmail.TabIndex = 8;
            // 
            // PWLabel
            // 
            PWLabel.AutoSize = true;
            PWLabel.ForeColor = Color.White;
            PWLabel.Location = new Point(20, 250);
            PWLabel.Name = "PWLabel";
            PWLabel.Size = new Size(91, 25);
            PWLabel.TabIndex = 9;
            PWLabel.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.Black;
            txtPassword.Location = new Point(132, 247);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(240, 31);
            txtPassword.TabIndex = 10;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.White;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Popup;
            btnRegister.ForeColor = Color.Black;
            btnRegister.Location = new Point(130, 290);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(102, 38);
            btnRegister.TabIndex = 11;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // SignupForm
            // 
            AcceptButton = btnRegister;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(409, 386);
            Controls.Add(panel1);
            Name = "SignupForm";
            Text = "Sign Up";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }
    }
}
