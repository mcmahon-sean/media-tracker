﻿using System;
using System.Windows.Forms;
using Supabase;
using media_tracker_desktop.Models;
using media_tracker_desktop.Models.SupabaseFunctionObjects;

namespace media_tracker_desktop.Forms
{
    public partial class SignupForm : Form
    {
        public event EventHandler RegistrationSucceeded;

        public SignupForm()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                var newUser = new UserRegistrationParam(
                    txtUsername.Text.Trim(),
                    txtFN.Text.Trim(),
                    txtLN.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtPassword.Text.Trim()
                );

                var (created, message) = await CreateUserFunction.CreateUser(newUser);

                if (created)
                {
                    RegistrationSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show(message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
