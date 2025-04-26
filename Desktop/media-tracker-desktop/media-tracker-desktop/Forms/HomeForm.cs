using System;
using System.Windows.Forms;

namespace media_tracker_desktop.Forms
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            using (var login = new SigninForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    new DashboardForm().Show();
                    this.Hide();
                }
            }
        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            using (var register = new SignupForm())
            {
                register.ShowDialog();
            }
        }
    }
}
