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
            this.signInButton = new System.Windows.Forms.Button();
            this.signUpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // signInButton
            // 
            this.signInButton.Location = new System.Drawing.Point(50, 30);
            this.signInButton.Name = "signInButton";
            this.signInButton.Size = new System.Drawing.Size(100, 40);
            this.signInButton.Text = "Sign In";
            this.signInButton.Click += new System.EventHandler(this.signInButton_Click);
            // 
            // signUpButton
            // 
            this.signUpButton.Location = new System.Drawing.Point(200, 30);
            this.signUpButton.Name = "signUpButton";
            this.signUpButton.Size = new System.Drawing.Size(100, 40);
            this.signUpButton.Text = "Sign Up";
            this.signUpButton.Click += new System.EventHandler(this.signUpButton_Click);
            // 
            // HomeForm
            // 
            this.ClientSize = new System.Drawing.Size(360, 100);
            this.Controls.Add(this.signInButton);
            this.Controls.Add(this.signUpButton);
            this.Name = "HomeForm";
            this.Text = "Welcome";
            this.ResumeLayout(false);
        }
    }
}
