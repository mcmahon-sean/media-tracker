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
using media_tracker_desktop.Models.TMDB;
using System.Data;

namespace media_tracker_desktop.Forms
{
    public partial class LinkLastFmForm : Form
    {
        private readonly LastFmService _lastFm = new LastFmService();
        private List<UserFavoriteMedia> _favorites = [];

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
                        (bool success1, List<LastFM_Artist>? topArtists) = await LastFMApi.GetUserTopArtists();
                        (bool success2, List<LastFM_Track>? recentTracks) = await LastFMApi.GetUserRecentTracks();
                        BuildViewGrid(topArtists, recentTracks);
                    }
                    else if (user == null)
                    {
                        MessageBox.Show("LastFM account not found.");
                        pnlLink.Visible = true;
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

        public async void BuildViewGrid(List<LastFM_Artist> topArtists, List<LastFM_Track> recentTracks)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID");//probably won't need this
            table.Columns.Add("Top Type");
            table.Columns.Add("Artist Name");
            table.Columns.Add("Top Track");

            foreach (LastFM_Artist artist in topArtists)
            {
                table.Rows.Add(artist.Mbid, "Top Artist",artist.Name, null);
            }
            foreach (LastFM_Track track in recentTracks)
            {
                table.Rows.Add("", "Top Track", track.ArtistName, track.Name);
            }

            lastFmDataGridView.DataSource = table;

            lastFmDataGridView.Columns["ID"].Visible = false;
            lastFmDataGridView.RowHeadersVisible = false;
            lastFmDataGridView.AllowUserToAddRows = false;

            lastFmDataGridView.Columns["Artist Name"].Width = 200;
            lastFmDataGridView.Columns["Top Track"].Width = 200;

            DataGridViewButtonColumn favoriteButtons = new DataGridViewButtonColumn();
            // Add the button properties.
            favoriteButtons.Name = "btnFavorite";
            // Header text is displayed as the column title.
            favoriteButtons.HeaderText = " ";
            favoriteButtons.FlatStyle = FlatStyle.Popup;

            // Add the button column to the data grid.
            lastFmDataGridView.Columns.Add(favoriteButtons);

            // Retrieve the list of user's favorite media.
            _favorites = await UserAppAccount.GetFavoriteMediaList();

            // Foreach row in the data grid,
            foreach (DataGridViewRow row in lastFmDataGridView.Rows)
            {
                // Retrieve the current game ID.
                string currentRowTitle = (string)row.Cells["Artist Name"].Value;

                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = "\u2730";

                // Retrieve the game with the same artist/track name from the favorite list.
                // Make sure that it is from the lastFM platform.
                var favoriteMusic = _favorites.FirstOrDefault(
                    g => ((g.Title == currentRowTitle) || (g.Artist == currentRowTitle)) && (g.MediaTypeID == 4) || (g.MediaTypeID == 6));

                // If a game exist, mark the button as favorite.
                if (favoriteMusic != null)
                {
                    row.Cells["btnFavorite"].Value = "\u2605";
                }
            }

            // Subscribe to event handler that specifies what happens when any of the favorite buttons are clicked.
            lastFmDataGridView.CellClick += btnFavorite_CellClick;
        }


        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var currentButton = lastFmDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != lastFmDataGridView.Columns["btnFavorite"].Index)
                {
                    return;
                }

                // Retrieve the current artist or track name.
                string currentRowTitle = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Artist Name"].Value;

                // Retrieve the show with the same show ID from the favorite list.
                // Make sure that it is from the TMDB platform.
                var favoriteShow = _favorites.FirstOrDefault(
                    g => g.Title == currentRowTitle && ((g.MediaTypeID == 4) || (g.MediaTypeID == 6)));

                // If there is no game,
                if (favoriteShow == null)
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

                // Retrieve the title of the current row.

                var format = UserAppAccount.MediaTypeID.Song;
                if ((string)lastFmDataGridView.Rows[e.RowIndex].Cells["Top Type"].Value == "Top Artist")
                {
                    format = UserAppAccount.MediaTypeID.Artist;
                }

                // Update the favorite status of the media.
                // The SP unfavorites/favorites media.
                await UserAppAccount.FavoriteMedia(
                    platformID: UserAppAccount.LastFMPlatformID,
                    username: UserAppAccount.Username,
                    mediaTypeID: format,
                    title: currentRowTitle,
                    artist: currentRowTitle
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