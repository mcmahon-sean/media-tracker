using System;
using System.Windows.Forms;
using Supabase;
using media_tracker_desktop.Models;

namespace media_tracker_desktop.Forms
{
    public partial class SigninForm : Form
    {
        public event EventHandler LoginSucceeded;
        public event EventHandler SwitchToRegister;

        public SigninForm()
        {
            InitializeComponent();

            UserAppAccount.ConnectToDB(SupabaseConnection.GetClient());
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            var user = new UserLoginParam(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            var (success, message) = await UserAppAccount.AuthenticateUser(user);

            if (success)
                LoginSucceeded?.Invoke(this, EventArgs.Empty);
                
            else
                MessageBox.Show(message, "Sign In Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            SwitchToRegister?.Invoke(this, EventArgs.Empty);
        }
    }
}
