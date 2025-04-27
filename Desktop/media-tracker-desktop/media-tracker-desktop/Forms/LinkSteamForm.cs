using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using Supabase;
using media_tracker_desktop.Models;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.ApiModels;

namespace media_tracker_desktop.Forms
{
    public partial class LinkSteamForm : Form
    {
        //private readonly SteamService _steam = new SteamService();

        public LinkSteamForm()
        {
            InitializeComponent();

            // If user has a steam id,
            if (!string.IsNullOrEmpty(UserAppAccount.UserSteamID))
                // Load display.
                _ = LoadSteamAsync();
        }

        private async Task LoadSteamAsync()
        {
            // Remove link panel.
            pnlLink.Visible = false;

            try
            {
                // If user has a steam account linked,
                if (!string.IsNullOrEmpty(UserAppAccount.UserSteamID))
                {
                    // Retrieve owned games.
                    (bool success, List<Steam_Game>? games) = await SteamApi.GetOwnedGames(); 

                    // If success,
                    if (success)
                    {
                        // Display owned games.
                        steamDataGridView.DataSource = games;
                    }
                }
                // If user doesn't have steam account linked,
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

            // Retrieve user's steam id.
            var steamId = steamTextBox.Text.Trim();

            // If the user inputted a steam id,
            if (!string.IsNullOrEmpty(steamId))
            {
                // Determine if steam account is already linked with current user.
                bool steamNotLinked = string.IsNullOrEmpty(UserAppAccount.UserSteamID);

                // If steam is not linked,
                if (steamNotLinked)
                {
                    // Add third party id.
                    var (added, msg) = await UserAppAccount.AddThirdPartyId(
                        UserAppAccount.SteamPlatformID,
                        steamId
                    );

                    // If added,
                    if (added)
                    {
                        // Load display.
                        await LoadSteamAsync();
                    }
                    else
                    {
                        MessageBox.Show($"Link failed: {msg}");

                        // Load display.
                        await LoadSteamAsync();
                    }
                }
                else
                {
                    MessageBox.Show($"Steam account is already linked with {UserAppAccount.Username}");
                }
            }
            else
            {
                MessageBox.Show("Please enter a Steam ID.");
            }
        }
    }
}