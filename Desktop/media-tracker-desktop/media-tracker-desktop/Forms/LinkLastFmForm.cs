using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using media_tracker_desktop.Models.LastFM;
using Supabase;
using media_tracker_desktop.Models;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.Steam;

namespace media_tracker_desktop.Forms
{
    public partial class LinkLastFmForm : Form
    {
        //private readonly LastFmService _lastFm = new LastFmService();

        public LinkLastFmForm()
        {
            InitializeComponent();

            // If user has a LastFM id,
            if (!string.IsNullOrEmpty(UserAppAccount.UserLastFmID))
                // Load display.
                _ = LoadLastFmAsync();
        }

        private async Task LoadLastFmAsync()
        {
            // Remove link panel.
            pnlLink.Visible = false;

            try
            {
                // If user has a LastFM account linked,
                if (!string.IsNullOrEmpty(UserAppAccount.UserLastFmID))
                {
                    // Retrieve user information.
                    (bool success, LastFM_User? user) = await LastFMApi.GetUserInfo();

                    // If success,
                    if (success && user != null)
                    {
                        // Put the user object into a list since data grid view accepts a list for it to work.
                        // Display user information.
                        lastFmDataGridView.DataSource = new List<LastFM_User> { user };
                    }
                }
                // If user doesn't have LastFM account linked,
                else
                {
                    // Display link panel.
                    pnlLink.Visible = true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}");
            }
        }


        private async void linkButton_Click(object sender, EventArgs e)
        {
            // Ensure user is logged in.
            if (!UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show("Please Sign-In first.");
                return;
            }

            var username = lastFmTextBox.Text.Trim();

            // If the user inputted a LastFM username,
            if (!string.IsNullOrEmpty(username))
            {
                // Determine if LastFM account is already linked with current user.
                bool lastFmNotLinked = string.IsNullOrEmpty(UserAppAccount.UserLastFmID);

                // If LastFM is not linked,
                if (lastFmNotLinked)
                {
                    // Add third party id.
                    var (added, msg) = await UserAppAccount.AddThirdPartyId(
                        UserAppAccount.LastFMPlatformID,
                        username);

                    // If added,
                    if (added)
                    {
                        // Load display.
                        await LoadLastFmAsync();
                    }
                    else
                    {
                        MessageBox.Show($"Link failed: {msg}");

                        // Load display.
                        await LoadLastFmAsync();
                    }
                }
                else
                {
                    MessageBox.Show($"LastFM account is already linked with {UserAppAccount.Username}");
                }
            }
            else
            {
                MessageBox.Show("Please enter a LastFM Username.");
            }
        }
    }
}