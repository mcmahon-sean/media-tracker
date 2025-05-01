using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using Supabase;
using media_tracker_desktop.Models;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.ApiModels;
using System.Data;
using media_tracker_desktop.Models.SupabaseTables;

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

        private List<UserFavoriteMedia> _favorites = [];

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
                        await DisplayGames(games ?? []);   
                    }
                    else
                    {
                        await DisplayGames([]);
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

        // Method: Display the list of games.
        private async Task DisplayGames(List<Steam_Game> games)
        {
            // Create a table to be used for the data grid.
            DataTable table = new DataTable();

            // Add the columns to be displayed.
            table.Columns.Add("AppID");
            table.Columns.Add("Title");

            if (games.Count <= 0 || games == null)
            {
                steamDataGridView.DataSource = table;
                MessageBox.Show("You don't own any games.");
                return;
            }

            // Add the rows to be displayed,
            foreach (Steam_Game game in games)
            {
                table.Rows.Add(game.AppID, game.Name);
            }

            // Add the table.
            steamDataGridView.DataSource = table;
            // Set the AppID row to not be visible to users.
            // This is to retrieve the game's ID from the row in which the favorite button is clicked.
            steamDataGridView.Columns["AppID"].Visible = false;

            // Create a data grid button column.
            DataGridViewButtonColumn favoriteButtons = new DataGridViewButtonColumn();

            // Add the button properties.
            favoriteButtons.Name = "btnFavorite";
            // Header text is displayed as the column title.
            favoriteButtons.HeaderText = " ";

            // Add the button column to the data grid.
            steamDataGridView.Columns.Add(favoriteButtons);

            // Retrieve the list of user's favorite media.
            _favorites = await UserAppAccount.GetFavoriteMediaList();

            // Foreach row in the data grid,
            foreach (DataGridViewRow row in steamDataGridView.Rows)
            {
                // Retrieve the current game ID.
                string currentRowAppID = (string)row.Cells["AppID"].Value;

                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = "\u2730";

                // Retrieve the game with the same game ID from the favorite list.
                // Make sure that it is from the Steam platform.
                var favoriteGame = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.SteamPlatformID);

                // If a game exist, mark the button as favorite.
                if (favoriteGame != null)
                {
                    row.Cells["btnFavorite"].Value = "\u2605";
                }
            }

            // Subscribe to event handler that specifies what happens when any of the favorite buttons are clicked.
            steamDataGridView.CellClick += btnFavorite_CellClick;
        }

        // Event Handler for Favorite Buttons in the Button Column of the data grid view.
        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var currentButton = steamDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != steamDataGridView.Columns["btnFavorite"].Index)
                {
                    return;
                }

                // Retrieve the current game ID.
                string currentRowAppID = (string)steamDataGridView.Rows[e.RowIndex].Cells["AppID"].Value;

                // Retrieve the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                // Retrieve the game with the same game ID from the favorite list.
                // Make sure that it is from the Steam platform.
                var favoriteGame = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.SteamPlatformID);

                // If there is no game,
                if (favoriteGame == null)
                {
                    // Fill in the star.
                    currentButton.Value = "\u2605";
                }
                // Else,
                else
                {
                    // Empty the star.
                    currentButton.Value = "\u2730";
                }

                // Retrieve the game title of the current row.
                string currentRowTitle = (string)steamDataGridView.Rows[e.RowIndex].Cells["Title"].Value;

                // Update the favorite status of the media.
                // The SP unfavorites/favorites media.
                await UserAppAccount.FavoriteMedia(
                    platformID: UserAppAccount.SteamPlatformID,
                    username: UserAppAccount.Username,
                    mediaTypeID: UserAppAccount.MediaTypeID.Game,
                    mediaPlatformID: currentRowAppID,
                    title: currentRowTitle
                    );
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