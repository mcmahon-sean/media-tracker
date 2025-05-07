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
using static System.Net.Mime.MediaTypeNames;

namespace media_tracker_desktop.Forms
{
    public partial class LinkTmdbForm : Form
    {
        private readonly string[] SORT_OPTIONS_FAVORITE_TV_SHOW_ASC = ["Favorite (asc)", "ID (asc)", "Name (asc)"];
        private readonly string[] SORT_OPTIONS_FAVORITE_TV_SHOW_DESC = ["Favorite (desc)", "ID (desc)", "Name (desc)"];

        private readonly string[] SORT_OPTIONS_FAVORITE_MOVIE_ASC = ["Favorite (asc)", "ID (asc)", "Name (asc)"];
        private readonly string[] SORT_OPTIONS_FAVORITE_MOVIE_DESC = ["Favorite (desc)", "ID (desc)", "Name (desc)"];

        private const string FILLED_STAR = "\u2605";
        private const string EMPTY_STAR = "\u2730";

        private DataTable _tableData = new DataTable();
        private Panel? _pnlSearchAndSort = null;
        private TextBox? _txtSearch = null;
        private Button _btnSort = new Button();
        private ContextMenuStrip? _sortMenu = null;

        private bool _tmdbSortVisible = false;
        private List<UserFavoriteMedia> _favorites = [];
        private List<TMDB_TV_Show> _tvShows = [];
        private List<TMDB_Movie> _movies = [];

        private string _dataOption = "";

        public LinkTmdbForm(string option = "")
        {
            InitializeComponent();

            // Store the endpoint option.
            _dataOption = option;

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
                    await BuildViewGridBasedOnOption();

                    // Subscribe to event handlers:
                    // When any of the favorite buttons are clicked.
                    tmdbDataGridView.CellClick += btnFavorite_CellClick!;
                    // When any sort items in the sort menu are clicked.
                    _sortMenu.ItemClicked += sortMenu_ItemClicked!;
                    // When the sort menu button is clicked.
                    _btnSort.Click += btnSort_Click!;
                    // When user presses a button in search bar.
                    _txtSearch.KeyDown += txtSearch_KeyDown!;
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

        /// <summary>
        /// Builds display based on endpoint option.
        /// </summary>
        private async Task BuildViewGridBasedOnOption()
        {
            if (_pnlSearchAndSort == null)
            {
                BuildSearchAndSortPanel();
            }

            if (_dataOption == MainForm.TMDBOptions[0])
            {
                // Retrieve data.
                (bool isTVSuccess, List<TMDB_TV_Show>? tvShows) = await TmdbApi.GetUserFavoriteTV();

                // Save data.
                _tvShows = tvShows ?? [];

                if (_sortMenu == null)
                {
                    _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_FAVORITE_TV_SHOW_ASC);
                }
                if (_txtSearch != null)
                {
                    _txtSearch.PlaceholderText = "Search for name...";
                }

                // Build display.
                BuildFavoriteTVShowViewGrid(_tvShows);
            }
            else if (_dataOption == MainForm.TMDBOptions[1])
            {
                // Retrieve data.
                (bool isMovieSuccess, List<TMDB_Movie>? movies) = await TmdbApi.GetUserFavoriteMovies();

                // Save data.
                _movies = movies ?? [];

                if (_sortMenu == null)
                {
                    _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_FAVORITE_MOVIE_ASC);
                }
                if (_txtSearch != null)
                {
                    _txtSearch.PlaceholderText = "Search for title...";
                }

                // Build display.
                BuildFavoriteMovieViewGrid(_movies);
            }
        }

        private void BuildFavoriteTVShowViewGrid(List<TMDB_TV_Show> tvShows)
        {
            _tableData = new DataTable();

            // Columns:
            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Name");

            // Return if no favorite tv shows.
            if (_tvShows.Count <= 0 || _tvShows == null)
            {
                MessageBox.Show("You don't have any favorite tv show.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Foreach data,
            foreach (TMDB_TV_Show tvShow in tvShows)
            {
                // Add row.
                _tableData.Rows.Add(tvShow.ID, tvShow.Name);
            }

            tmdbDataGridView.DataSource = _tableData;

            // Configurations:
            //tmdbDataGridView.Columns["ID"].Visible = false;
            tmdbDataGridView.RowHeadersVisible = false;
            tmdbDataGridView.AllowUserToAddRows = false;
            tmdbDataGridView.ReadOnly = true;

            tmdbDataGridView.Columns["ID"].Width = 200;
            tmdbDataGridView.Columns["Name"].Width = 200;

            BuildFavoriteButtonColumn();
        }

        private void BuildFavoriteMovieViewGrid(List<TMDB_Movie> movies)
        {
            _tableData = new DataTable();

            // Columns:
            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Title");

            // Return if no favorite movies.
            if (_movies.Count <= 0 || _movies == null)
            {
                MessageBox.Show("You don't have any favorite movie.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Foreach data,
            foreach (TMDB_Movie movie in movies)
            {
                // Add row.
                _tableData.Rows.Add(movie.ID, movie.Title);
            }

            tmdbDataGridView.DataSource = _tableData;

            // Configurations:
            //tmdbDataGridView.Columns["ID"].Visible = false;
            tmdbDataGridView.RowHeadersVisible = false;
            tmdbDataGridView.AllowUserToAddRows = false;
            tmdbDataGridView.ReadOnly = true;

            tmdbDataGridView.Columns["ID"].Width = 200;
            tmdbDataGridView.Columns["Title"].Width = 200;

            BuildFavoriteButtonColumn();
        }

        private async void BuildFavoriteButtonColumn()
        {
            AppElement.AddFavoriteButtonColumn(tmdbDataGridView);

            // Retrieve the list of user's favorite media.
            _favorites = await UserAppAccount.GetFavoriteMediaList();

            // Foreach row in the data grid,
            foreach (DataGridViewRow row in tmdbDataGridView.Rows)
            {
                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = EMPTY_STAR;

                // Retrieve the current ID for movie or tv show.
                string currentRowAppID = (string)row.Cells["ID"].Value;

                // Retrieve the tv show/movie with the same ID from the favorite list.
                // Make sure that it is from the TMDB platform.
                var favoriteMedia = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.TMDBPlatformID);

                // If a tv show/movie exist, mark the button as favorite.
                if (favoriteMedia != null)
                {
                    row.Cells["btnFavorite"].Value = FILLED_STAR;
                }
            }
        }

        // Event: When a favorite button is clicked.
        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != tmdbDataGridView.Columns["btnFavorite"].Index)
                {
                    // Build favorite column after sorting using column header.
                    if (e.RowIndex == -1)
                    {
                        BuildFavoriteButtonColumn();
                    }
                    return;
                }

                // Retrieve the clicked button.
                var currentButton = tmdbDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Retrieve the current ID.
                string currentRowAppID = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["ID"].Value;

                // Update the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                // Retrieve the media with the same ID from the favorite list.
                // Make sure that it is from the TMDB platform.
                var favoriteMedia = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.TMDBPlatformID);

                // If there is no media,
                if (favoriteMedia == null)
                {
                    // Fill in the star.
                    currentButton.Value = FILLED_STAR;
                }
                // Else,
                else
                {
                    // Empty the star.
                    currentButton.Value = EMPTY_STAR;
                }

                if (_dataOption == MainForm.TMDBOptions[0])
                {
                    // Retrieve the media name of the current row.
                    string currentRowName = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["Name"].Value;

                    // Update the favorite status of the media.
                    // The SP unfavorites/favorites media.
                    await UserAppAccount.FavoriteMedia(
                        platformID: UserAppAccount.TMDBPlatformID,
                        username: UserAppAccount.Username,
                        mediaTypeID: UserAppAccount.MediaTypeID.TV_Show,
                        mediaPlatformID: currentRowAppID,
                        title: currentRowName
                        );
                }
                else if (_dataOption == MainForm.TMDBOptions[1])
                {
                    // Retrieve the media title of the current row.
                    string currentRowTitle = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["Title"].Value;

                    // Update the favorite status of the media.
                    // The SP unfavorites/favorites media.
                    await UserAppAccount.FavoriteMedia(
                        platformID: UserAppAccount.TMDBPlatformID,
                        username: UserAppAccount.Username,
                        mediaTypeID: UserAppAccount.MediaTypeID.Film,
                        mediaPlatformID: currentRowAppID,
                        title: currentRowTitle
                        );
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}");
            }
        }

        // Method: Build the search and sort panel.
        private void BuildSearchAndSortPanel()
        {
            // Build the panel.
            _pnlSearchAndSort = AppElement.GetSearchAndSortPanel();

            // Add to form.
            this.Controls.Add(_pnlSearchAndSort);

            // Retrieve the search box and sort button from panel.
            _txtSearch = (TextBox)_pnlSearchAndSort.Controls["txtSearch"]!;

            _btnSort = (Button)_pnlSearchAndSort.Controls["btnSort"]!;
        }

        // Event: When user presses a button in the search textbox.
        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // If user pressed enter,
            if (e.KeyCode == Keys.Enter)
            {
                TextBox txtSearch = (TextBox)sender;

                // Retrieve search string.
                string searchString = txtSearch.Text.ToLower();

                // If there's a search string,
                if (!string.IsNullOrEmpty(searchString))
                {
                    // Search for data.
                    SearchData(searchString);
                }
                // Else,
                else
                {
                    // Reset display.
                    await BuildViewGridBasedOnOption();
                }
            }
        }

        // Method: Search for data.
        private void SearchData(string text)
        {
            text.ToLower();

            if (_dataOption == MainForm.TMDBOptions[0])
            {
                // Query options:
                // Ensure that the search is case insensitive.
                QueryOptions<TMDB_TV_Show> optionTvShow = new QueryOptions<TMDB_TV_Show>
                {
                    Where = t => t.Name.ToLower().Contains(text)
                };

                // Retrieve data.
                List<TMDB_TV_Show> resultTvShows = DataFunctions.Sort(_tvShows, optionTvShow) ?? [];

                BuildFavoriteTVShowViewGrid(resultTvShows);
            }
            else if (_dataOption == MainForm.TMDBOptions[1])
            {
                // Query options:
                // Ensure that the search is case insensitive.
                QueryOptions<TMDB_Movie> optionMovie = new QueryOptions<TMDB_Movie>
                {
                    Where = m => m.Title.ToLower().Contains(text)
                };

                // Retrieve data.
                List<TMDB_Movie> resultMovies = DataFunctions.Sort(_movies, optionMovie) ?? [];

                BuildFavoriteMovieViewGrid(resultMovies);
            }
        }

        // Event: When sort button is clicked.
        private void btnSort_Click(object sender, EventArgs e)
        {
            if (_sortMenu == null)
            {
                MessageBox.Show("Sort menu didn't show up.");
                return;
            }

            // Retrieve sort button.
            _btnSort = (Button)sender;

            // If the button is clicked to open the menu,
            if (!_tmdbSortVisible)
            {
                // Open the menu.
                _sortMenu.Show(_btnSort, new Point(0, _btnSort.Height));

                // Menu is visible.
                _tmdbSortVisible = true;
            }
            // If the button is clicked to close the menu,
            else
            {
                // Close the menu.
                _sortMenu.Close();

                // Menu is not visible.
                _tmdbSortVisible = false;
            }
        }

        // Event: When a sort item is clicked in the sort menu,
        private void sortMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Retrieve the sort item.
            string? option = e.ClickedItem?.Text;

            // If there is an item,
            if (option != null)
            {
                // Update the button to show the sort item selected.
                _btnSort.Text = option;

                // Sort lastFM display.
                SortData(option);
            }
        }

        // Method: Sorts the data based on sort option.
        private void SortData(string option)
        {
            // Display Option 1
            if (_dataOption == MainForm.TMDBOptions[0])
            {
                List<TMDB_TV_Show> sortedTVShows = [];

                // Sorting Option 1
                if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[0])
                {
                    // Sort data.
                    sortedTVShows = SortFavoriteTVShow("asc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[0];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
                else if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[0])
                {
                    // Sort data.
                    sortedTVShows = SortFavoriteTVShow("desc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[0];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
                // Sorting Option 2
                if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[1])
                {
                    QueryOptions<TMDB_TV_Show> options = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.ID,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedTVShows = DataFunctions.Sort(_tvShows, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[1];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
                else if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[1])
                {
                    QueryOptions<TMDB_TV_Show> options = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.ID,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedTVShows = DataFunctions.Sort(_tvShows, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[1];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
                // Sorting Option 3
                if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[2])
                {
                    QueryOptions<TMDB_TV_Show> options = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedTVShows = DataFunctions.Sort(_tvShows, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[2];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
                else if (option == SORT_OPTIONS_FAVORITE_TV_SHOW_DESC[2])
                {
                    QueryOptions<TMDB_TV_Show> options = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedTVShows = DataFunctions.Sort(_tvShows, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_FAVORITE_TV_SHOW_ASC[2];

                    // Display.
                    BuildFavoriteTVShowViewGrid(sortedTVShows);
                }
            }
            // Display Option 2
            else if (_dataOption == MainForm.TMDBOptions[1])
            {
                List<TMDB_Movie> sortedMovies = [];

                // Sorting Option 1
                if (option == SORT_OPTIONS_FAVORITE_MOVIE_ASC[0])
                {
                    // Sort data.
                    sortedMovies = SortFavoriteMovie("asc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_FAVORITE_MOVIE_DESC[0];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
                else if (option == SORT_OPTIONS_FAVORITE_MOVIE_DESC[0])
                {
                    // Sort data.
                    sortedMovies = SortFavoriteMovie("desc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_FAVORITE_MOVIE_ASC[0];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
                // Sorting Option 2
                if (option == SORT_OPTIONS_FAVORITE_MOVIE_ASC[1])
                {
                    QueryOptions<TMDB_Movie> options = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = t => t.ID,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedMovies = DataFunctions.Sort(_movies, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_FAVORITE_MOVIE_DESC[1];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
                else if (option == SORT_OPTIONS_FAVORITE_MOVIE_DESC[1])
                {
                    QueryOptions<TMDB_Movie> options = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = t => t.ID,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedMovies = DataFunctions.Sort(_movies, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_FAVORITE_MOVIE_ASC[1];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
                // Sorting Option 3
                if (option == SORT_OPTIONS_FAVORITE_MOVIE_ASC[2])
                {
                    QueryOptions<TMDB_Movie> options = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = t => t.Title,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedMovies = DataFunctions.Sort(_movies, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_FAVORITE_MOVIE_DESC[2];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
                else if (option == SORT_OPTIONS_FAVORITE_MOVIE_DESC[2])
                {
                    QueryOptions<TMDB_Movie> options = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = t => t.Title,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedMovies = DataFunctions.Sort(_movies, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_FAVORITE_MOVIE_ASC[2];

                    // Display.
                    BuildFavoriteMovieViewGrid(sortedMovies);
                }
            }
        }

        private List<TMDB_TV_Show> SortFavoriteTVShow(string orderByDirection)
        {
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.TV_Show,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            List<TMDB_TV_Show> favorites = [];
            List<TMDB_TV_Show> unfavorites = [];

            foreach (TMDB_TV_Show tvShow in _tvShows)
            {
                // If tv show is in favorites,
                if (sortedFavorites.Any(f => f.MediaPlatID == tvShow.ID.ToString()))
                {
                    // Add it.
                    favorites.Add(tvShow);
                }
                else
                {
                    // Add to unfavorite list.
                    unfavorites.Add(tvShow);
                }
            }

            return DataFunctions.SortFavorite(orderByDirection, favorites, unfavorites);
        }

        private List<TMDB_Movie> SortFavoriteMovie(string orderByDirection)
        {
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Film,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            List<TMDB_Movie> favorites = [];
            List<TMDB_Movie> unfavorites = [];

            foreach (TMDB_Movie movie in _movies)
            {
                // If movie is in favorites,
                if (sortedFavorites.Any(f => f.MediaPlatID == movie.ID.ToString()))
                {
                    // Add it.
                    favorites.Add(movie);
                }
                else
                {
                    // Add to unfavorite list.
                    unfavorites.Add(movie);
                }
            }

            return DataFunctions.SortFavorite(orderByDirection, favorites, unfavorites);
        }

        private async void linkButton_Click(object sender, EventArgs e)
        {
            // Ensure user is logged in.
            if (!UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show("Please Sign-In first.", "Not Signed-In", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (HttpRequestException error)
            {
                if (error.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Permission denied.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}");
            }
        }
    }
}