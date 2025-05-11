using media_tracker_desktop.Models.SupabaseFunctionObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace media_tracker_desktop.Forms
{
    public partial class UpdateUserForm : Form
    {
        public UpdateUserForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Launch the edit user form.
        /// </summary>
        public void LaunchEditUserForm()
        {
            // Displays the current user's details.
            // Except for password, for security reasons unless justified.
            txtUsername.Text = UserAppAccount.Username;
            txtFirstName.Text = UserAppAccount.FirstName;
            txtLastName.Text = UserAppAccount.LastName;
            txtEmail.Text = UserAppAccount.Email;

            this.ShowDialog();
        }

        // Event: When user confirms edit.
        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            // Retrieve user's input.
            string newUsername = txtUsername.Text;
            string newFirstName = txtFirstName.Text;
            string newLastName = txtLastName.Text;
            string newEmail = txtEmail.Text;
            string newPassword = txtPassword.Text;

            // Update user.
            Dictionary<string, dynamic> result = await UpdateUserFunction.UpdateUser(newUsername, newFirstName, newLastName, newEmail, newPassword);

            // If error, display error message.
            if (result["status"] == "error")
            {
                MessageBox.Show(result["statusMessage"], "Error");
            }
            // Else,
            else
            {
                // Display success and close the form.
                MessageBox.Show(result["statusMessage"], "Updated");
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
