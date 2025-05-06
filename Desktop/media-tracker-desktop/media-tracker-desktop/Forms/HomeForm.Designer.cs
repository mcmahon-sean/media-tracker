namespace media_tracker_desktop.Forms
{
    partial class HomeForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button signInButton;
        private Button signUpButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            signInButton = new Button();
            signUpButton = new Button();
            SuspendLayout();
            // 
            // signInButton
            // 
            signInButton.BackColor = Color.White;
            signInButton.FlatStyle = FlatStyle.Popup;
            signInButton.ForeColor = Color.Black;
            signInButton.Location = new Point(50, 30);
            signInButton.Name = "signInButton";
            signInButton.Size = new Size(100, 40);
            signInButton.TabIndex = 0;
            signInButton.Text = "Sign In";
            signInButton.UseVisualStyleBackColor = false;
            signInButton.Click += signInButton_Click;
            // 
            // signUpButton
            // 
            signUpButton.BackColor = Color.White;
            signUpButton.FlatStyle = FlatStyle.Popup;
            signUpButton.ForeColor = Color.Black;
            signUpButton.Location = new Point(200, 30);
            signUpButton.Name = "signUpButton";
            signUpButton.Size = new Size(100, 40);
            signUpButton.TabIndex = 1;
            signUpButton.Text = "Sign Up";
            signUpButton.UseVisualStyleBackColor = false;
            signUpButton.Click += signUpButton_Click;
            // 
            // HomeForm
            // 
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(360, 100);
            Controls.Add(signInButton);
            Controls.Add(signUpButton);
            Name = "HomeForm";
            Text = "Welcome";
            ResumeLayout(false);
        }
    }
}
