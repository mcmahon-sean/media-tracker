using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Models.TMDB;
using Supabase;
using media_tracker_desktop.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.LastFM;
using System.Data;

namespace media_tracker_desktop.Forms
{
    public partial class LinkTmdbForm : Form
    {
        private List<UserFavoriteMedia> _favorites = [];

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
                        //get favorite tv and movies
                        (bool isTVSuccess, List<TMDB_TV_Show> shows) = await TmdbApi.GetUserFavoriteTV();
                        (bool isMovieSuccess, List<TMDB_Movie> movies) = await TmdbApi.GetUserFavoriteMovies();
                        BuildViewGrid(shows ?? [], movies ?? []);
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

        public async void BuildViewGrid(List<TMDB_TV_Show> shows, List<TMDB_Movie> movies)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("Title");
            table.Columns.Add("Format");

            foreach (TMDB_TV_Show show in shows)
            {
                table.Rows.Add(show.ID, show.Name, "TV Show");
            }
            foreach (TMDB_Movie movie in movies)
            {
                table.Rows.Add(movie.ID, movie.Title, "Movie");
            }

            tmdbDataGridView.DataSource = table;

            tmdbDataGridView.Columns["ID"].Visible = false;
            tmdbDataGridView.RowHeadersVisible = false;
            tmdbDataGridView.AllowUserToAddRows = false;

            tmdbDataGridView.Columns["Title"].Width = 200;

            DataGridViewButtonColumn favoriteButtons = new DataGridViewButtonColumn();
            // Add the button properties.
            favoriteButtons.Name = "btnFavorite";
            // Header text is displayed as the column title.
            favoriteButtons.HeaderText = " ";
            favoriteButtons.FlatStyle = FlatStyle.Popup;

            // Add the button column to the data grid.
            tmdbDataGridView.Columns.Add(favoriteButtons);

            // Retrieve the list of user's favorite media.
            _favorites = await UserAppAccount.GetFavoriteMediaList();

            // Foreach row in the data grid,
            foreach (DataGridViewRow row in tmdbDataGridView.Rows)
            {
                // Retrieve the current game ID.
                string currentRowAppID = (string)row.Cells["ID"].Value;

                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = "\u2730";

                // Retrieve the game with the same game ID from the favorite list.
                // Make sure that it is from the Steam platform.
                var favoriteGame = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.TMDBPlatformID);

                // If a game exist, mark the button as favorite.
                if (favoriteGame != null)
                {
                    row.Cells["btnFavorite"].Value = "\u2605";
                }
            }

            // Subscribe to event handler that specifies what happens when any of the favorite buttons are clicked.
            tmdbDataGridView.CellClick += btnFavorite_CellClick;
        }

        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != tmdbDataGridView.Columns["btnFavorite"].Index)
                {
                    return;
                }

                var currentButton = tmdbDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Retrieve the current tmdb ID.
                string currentRowAppID = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["ID"].Value;

                // Retrieve the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                // Retrieve the show with the same show ID from the favorite list.
                // Make sure that it is from the TMDB platform.
                var favoriteShow = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.TMDBPlatformID);

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

                // Retrieve the game title of the current row.
                string currentRowTitle = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["Title"].Value;

                var format = UserAppAccount.MediaTypeID.TV_Show;
                if ((string)tmdbDataGridView.Rows[e.RowIndex].Cells["Format"].Value == "Movie")
                {
                    format = UserAppAccount.MediaTypeID.Film;
                }

                // Update the favorite status of the media.
                // The SP unfavorites/favorites media.
                await UserAppAccount.FavoriteMedia(
                    platformID: UserAppAccount.TMDBPlatformID,
                    username: UserAppAccount.Username,
                    mediaTypeID: format,
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