using System;
using System.Windows.Forms;

namespace media_tracker_desktop.Forms
{
    public partial class DashboardForm : Form
    {
        public string Username { get; set; } = string.Empty;

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {Username}!";
        }
    }
}
