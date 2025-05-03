using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                // Retrieve the type of the row. (Top Artist or Top Track)
                string currentRowTopType = (string)row.Cells["Top Type"].Value;

                // Default: Unfavorite button.
                row.Cells["btnFavorite"].Value = "\u2730";

                // If type is Top Artist,
                if (currentRowTopType == "Top Artist")
                {
                    // Retrieve the current artist name.
                    string currentRowArtistName = (string)row.Cells["Artist Name"].Value;

                    // Retrieve the artist with the same name from the favorite list.
                    // Ensure that the record is of the Artist media type.
                    var favoriteArtist = _favorites.FirstOrDefault(a => a.Artist == currentRowArtistName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist);

                    // If there is an artist,
                    if (favoriteArtist != null)
                    {
                        // Favorite the button.
                        row.Cells["btnFavorite"].Value = "\u2605";
                    }
                }
                // If type is Top Track,
                else if (currentRowTopType == "Top Track")
                {
                    // Retrieve the current track name.
                    string currentRowTrackName = (string)row.Cells["Top Track"].Value;

                    // Retrieve the track with the same name from the favorite list.
                    // Ensure that the record is of the Song media type.
                    var favoriteTrack = _favorites.FirstOrDefault(t => t.Title == currentRowTrackName && t.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song);

                    // If there is a track,
                    if (favoriteTrack != null)
                    {
                        // Favorite the button.
                        row.Cells["btnFavorite"].Value = "\u2605";
                    }
                }
            }

            // Subscribe to event handler that specifies what happens when any of the favorite buttons are clicked.
            lastFmDataGridView.CellClick += btnFavorite_CellClick;
        }


        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Retrieve the clicked button.
                var currentButton = lastFmDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != lastFmDataGridView.Columns["btnFavorite"].Index)
                {
                    return;
                }

                // Retrieve the current type (Top Artist or Top Track).
                string currentRowTopType = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Top Type"].Value;

                // Retrieve the artist name, which matters for both favoriting the artist and the track.
                string currentRowArtistName = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Artist Name"].Value;

                // Update the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                // If current type is Top Artist,
                if (currentRowTopType == "Top Artist")
                {
                    // Retrieve the artist with the same name from the favorite list.
                    // Ensure that the record is of the Artist media type.
                    var favoriteArtist = _favorites.FirstOrDefault(a => a.Artist == currentRowArtistName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist);

                    // If there is no favorite artist,
                    if (favoriteArtist == null)
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

                    // Favorite the artist.
                    // In the DB, I saw that artist name was stored in the MediaPlatformID in lowercase.
                    // Artist name is also stored as title.
                    await UserAppAccount.FavoriteMedia(
                        platformID: UserAppAccount.LastFMPlatformID,
                        username: UserAppAccount.Username,
                        mediaTypeID: UserAppAccount.MediaTypeID.Artist,
                        mediaPlatformID: currentRowArtistName.ToLower(),
                        title: currentRowArtistName,
                        artist: currentRowArtistName
                    );
                }
                // If current type is Top Track,
                else if (currentRowTopType == "Top Track")
                {
                    // Retrieve the track name.
                    string currentRowTrackName = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Top Track"].Value;

                    // Retrieve the track with the same name from the favorite list.
                    // Ensure that the record is of the Song media type.
                    var favoriteTrack = _favorites.FirstOrDefault(a => a.Title == currentRowTrackName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song);

                    // If there is no favorite track,
                    if (favoriteTrack == null)
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

                    // Favorite the track.
                    await UserAppAccount.FavoriteMedia(
                        platformID: UserAppAccount.LastFMPlatformID,
                        username: UserAppAccount.Username,
                        mediaTypeID: UserAppAccount.MediaTypeID.Song,
                        title: currentRowTrackName,
                        artist: currentRowArtistName
                    );
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