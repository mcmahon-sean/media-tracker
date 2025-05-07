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
using media_tracker_desktop.Models.SupabaseFunctionObjects;

namespace media_tracker_desktop.Forms
{
    public partial class LinkTmdbForm : Form
    {
        private readonly string[] SORT_OPTIONS_ASC = ["Title (asc)", "Favorite (asc)"];
        private readonly string[] SORT_OPTIONS_DESC = ["Title (desc)", "Favorite (desc)"];

        private DataTable _tableData = new DataTable();
        private Panel _pnlSearchAndSort = new Panel();
        private TextBox _txtSearch = new TextBox();
        private Button _btnSort = new Button();
        private ContextMenuStrip _sortMenu = new ContextMenuStrip();

        private bool _tmdbSortVisible = false;
        private List<UserFavoriteMedia> _favorites = [];
        private List<TMDB_TV_Show> _tvShows = [];
        private List<TMDB_Movie> _movies = [];

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
                        (bool isTVSuccess, List<TMDB_TV_Show>? shows) = await TmdbApi.GetUserFavoriteTV();
                        (bool isMovieSuccess, List<TMDB_Movie>? movies) = await TmdbApi.GetUserFavoriteMovies();

                        _tvShows = shows ?? [];
                        _movies = movies ?? [];

                        BuildViewGrid(shows ?? [], movies ?? []);

                        BuildSearchAndSortPanel();

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

        public void BuildViewGrid(List<TMDB_TV_Show> shows, List<TMDB_Movie> movies)
        {
            _tableData = new DataTable();

            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Title");
            _tableData.Columns.Add("Format");

            foreach (TMDB_TV_Show show in shows)
            {
                _tableData.Rows.Add(show.ID, show.Name, "TV Show");
            }
            foreach (TMDB_Movie movie in movies)
            {
                _tableData.Rows.Add(movie.ID, movie.Title, "Movie");
            }

            tmdbDataGridView.DataSource = _tableData;

            tmdbDataGridView.Columns["ID"].Visible = false;
            tmdbDataGridView.RowHeadersVisible = false;
            tmdbDataGridView.AllowUserToAddRows = false;

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
                // Retrieve the current ID for movie or tv show.
                string currentRowAppID = (string)row.Cells["ID"].Value;

                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = "\u2730";

                // Retrieve the tv show/movie with the same ID from the favorite list.
                // Make sure that it is from the TMDB platform.
                var favoriteMedia = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.TMDBPlatformID);

                // If a tv show/movie exist, mark the button as favorite.
                if (favoriteMedia != null)
                {
                    row.Cells["btnFavorite"].Value = "\u2605";
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
                    currentButton.Value = "\u2605";
                }
                // Else,
                else
                {
                    // Empty the star.
                    currentButton.Value = "\u2730";
                }

                // Retrieve the media title of the current row.
                string currentRowTitle = (string)tmdbDataGridView.Rows[e.RowIndex].Cells["Title"].Value;

                // Determine the correct format.
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

        // Method: Build the search and sort panel.
        private void BuildSearchAndSortPanel()
        {
            // Build the panel.
            _pnlSearchAndSort = AppElement.GetSearchAndSortPanel();

            // Add to form.
            this.Controls.Add(_pnlSearchAndSort);

            // Retrieve the search box and sort button from panel.
            _txtSearch = (TextBox)_pnlSearchAndSort.Controls["txtSearch"]!;
            _txtSearch.PlaceholderText = "Search for title...";

            _btnSort = (Button)_pnlSearchAndSort.Controls["btnSort"]!;

            // Add sort menu.
            _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_ASC);
        }

        // Event: When user presses a button in the search textbox.
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
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
                    BuildViewGrid(_tvShows, _movies);
                }
            }
        }

        // Method: Search for data.
        private void SearchData(string text)
        {
            text.ToLower();

            // Query options:
            // Ensure that the search is case insensitive.
            QueryOptions<TMDB_TV_Show> optionTvShow = new QueryOptions<TMDB_TV_Show>
            {
                Where = t => t.Name.ToLower().Contains(text)
            };

            QueryOptions<TMDB_Movie> optionMovie = new QueryOptions<TMDB_Movie>
            {
                Where = m => m.Title.ToLower().Contains(text)
            };

            // Retrieve data.
            List<TMDB_TV_Show> resultTvShows = DataFunctions.Sort(_tvShows, optionTvShow) ?? [];
            List<TMDB_Movie> resultMovies = DataFunctions.Sort(_movies, optionMovie) ?? [];

            // Display data.
            BuildViewGrid(resultTvShows, resultMovies);
        }

        // Event: When sort button is clicked.
        private void btnSort_Click(object sender, EventArgs e)
        {
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
            List<TMDB_TV_Show>? sortedTvShows = [];
            List<TMDB_Movie>? sortedMovies = [];

            // If there is data to be sorted,
            if (_tvShows != null && _movies != null)
            {
                // First Sorting Option
                if (option == SORT_OPTIONS_ASC[0])
                {
                    QueryOptions<TMDB_TV_Show> optionTvShow = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "asc"
                    };

                    QueryOptions<TMDB_Movie> optionMovie = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = m => m.Title,
                        OrderByDirection = "asc"
                    };

                    sortedTvShows = DataFunctions.Sort(_tvShows, optionTvShow);
                    sortedMovies = DataFunctions.Sort(_movies, optionMovie);

                    _sortMenu.Items[0].Text = SORT_OPTIONS_DESC[0];
                }
                else if (option == SORT_OPTIONS_DESC[0])
                {
                    QueryOptions<TMDB_TV_Show> optionTvShow = new QueryOptions<TMDB_TV_Show>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "desc"
                    };

                    QueryOptions<TMDB_Movie> optionMovie = new QueryOptions<TMDB_Movie>
                    {
                        OrderBy = m => m.Title,
                        OrderByDirection = "desc"
                    };

                    sortedTvShows = DataFunctions.Sort(_tvShows, optionTvShow);
                    sortedMovies = DataFunctions.Sort(_movies, optionMovie);

                    _sortMenu.Items[0].Text = SORT_OPTIONS_ASC[0];
                }
                // Second Sorting Option
                else if (option == SORT_OPTIONS_ASC[1])
                {
                    (List<TMDB_TV_Show> tvShows, List<TMDB_Movie> movies) = RetrieveSortedFavorites("asc");

                    // Update list.
                    sortedTvShows = tvShows;
                    sortedMovies = movies;

                    // Update the menu item.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_DESC[1];
                }
                else if (option == SORT_OPTIONS_DESC[1])
                {
                    (List<TMDB_TV_Show> tvShows, List<TMDB_Movie> movies) = RetrieveSortedFavorites("desc");

                    // Update list.
                    sortedTvShows = tvShows;
                    sortedMovies = movies;

                    // Update the menu item.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_ASC[1];
                }

                // Build based on whether or not the list was updated.
                BuildViewGrid(sortedTvShows ?? [], sortedMovies ?? []);
            }
        }

        // Method: Sorts the favorites.
        private (List<TMDB_TV_Show>, List<TMDB_Movie>) RetrieveSortedFavorites(string orderByDirection)
        {
            // Sort options based on passed orderByDirection.
            // Also, only tv shows and movies.
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.TV_Show || m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Film,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            // Retrieve the sorted favorites.
            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            // Initialize lists.
            List<TMDB_TV_Show> favoriteTvShowList = [];
            List<TMDB_TV_Show> unfavoriteTvShowList = [];
            List<TMDB_Movie> favoriteMovieList = [];
            List<TMDB_Movie> unfavoriteMovieList = [];

            // Foreach tv show that is currently in display,
            foreach (TMDB_TV_Show tvShow in _tvShows)
            {
                // If that tv show is a favorite,
                if (_favorites.Any(f => f.MediaPlatID == tvShow.ID.ToString()))
                {
                    // Add it to the favorite list.
                    favoriteTvShowList.Add(tvShow);
                }
                // Else,
                else
                {
                    // Add to the unfavorite list.
                    unfavoriteTvShowList.Add(tvShow);
                }
            }

            // Foreach movie that is currently in display,
            foreach (TMDB_Movie movie in _movies)
            {
                // If that movie is a favorite,
                if (_favorites.Any(f => f.MediaPlatID == movie.ID.ToString()))
                {
                    // Add it to the favorite list.
                    favoriteMovieList.Add(movie);
                }
                // Else,
                else
                {
                    // Add to the unfavorite list.
                    unfavoriteMovieList.Add(movie);
                }
            }

            // If the order is asc,
            if (orderByDirection == "asc")
            {
                // Add list together so that the favorite is first.
                favoriteTvShowList.AddRange(unfavoriteTvShowList);
                favoriteMovieList.AddRange(unfavoriteMovieList);

                return (favoriteTvShowList, favoriteMovieList);
            }
            // If the order is desc,
            else if (orderByDirection == "desc")
            {
                // Add list together so that the favorite is last.
                unfavoriteTvShowList.AddRange(favoriteTvShowList);
                unfavoriteMovieList.AddRange(favoriteMovieList);

                return (unfavoriteTvShowList, unfavoriteMovieList);
            }
            else
            {
                return ([], []);
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
                        (bool added, string message) = await Add3rdPartyIDFunction.AddThirdPartyId(UserAppAccount.TMDBPlatformID, sessionId);

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