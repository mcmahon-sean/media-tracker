using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using media_tracker_desktop.Models.TMDB;
using Supabase;
using media_tracker_desktop.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.LastFM;

namespace media_tracker_desktop.Forms
{
    public partial class LinkTmdbForm : Form
    {
        //private readonly TmdbService _tmdbSvc = new TmdbService();

        public LinkTmdbForm()
        {
            InitializeComponent();

            // If user has a TMDB id,
            if (!string.IsNullOrEmpty(UserAppAccount.UserTmdbSessionID))
                // Load display.
                _ = LoadTmdbAsync();
        }

        private async Task LoadTmdbAsync()
        {
            // Remove link panel.
            pnlLink.Visible = false;

            try
            {
                // If user has a TMDB account linked,
                if (!string.IsNullOrEmpty(UserAppAccount.UserTmdbAccountID) && !string.IsNullOrEmpty(UserAppAccount.UserTmdbSessionID))
                {
                    // Retrieve user information.
                    (bool success, TMDB_Account? userAccount) = await TmdbApi.GetAccountDetails();

                    // If success,
                    if (success && userAccount != null)
                    {
                        // Put the user object into a list since data grid view accepts a list for it to work.
                        // Display user information.
                        tmdbDataGridView.DataSource = new List<TMDB_Account> { userAccount };
                    }
                }
                // If user doesn't have TMDB account linked,
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

            try
            {
                // Determine if TMDB account is already linked with current user.
                bool tmdbNotLinked = string.IsNullOrEmpty(UserAppAccount.UserTmdbAccountID) && string.IsNullOrEmpty(UserAppAccount.UserTmdbSessionID);

                // If TMDB is not linked,
                if (tmdbNotLinked)
                {
                    // Execute method for TMDB linking.
                    string? sessionId = await TmdbApi.RetrieveSessionID();

                    // If linking is successful and session ID is retrieved,
                    if (sessionId != null)
                    {
                        // Add third party id.
                        (bool added, string message) = await UserAppAccount.AddThirdPartyId(UserAppAccount.TMDBPlatformID, sessionId);

                        // If added,
                        if (added)
                        {
                            // Load display.
                            await LoadTmdbAsync();
                        }
                        else
                        {
                            MessageBox.Show($"Link failed: {message}");

                            // Load display.
                            await LoadTmdbAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to link TMDB account, please try again.");
                    }
                }
                else
                {
                    MessageBox.Show($"TMDB account is already linked with {UserAppAccount.Username}");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}");
            }
        }
    }
}