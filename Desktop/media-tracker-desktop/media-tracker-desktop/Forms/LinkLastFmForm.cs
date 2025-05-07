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
using System.Windows.Automation;
using System.Windows.Controls.Ribbon;
using System.DirectoryServices.ActiveDirectory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Documents;
using System.Linq.Expressions;
using System.Linq;
using media_tracker_desktop.Models.SupabaseFunctionObjects;

namespace media_tracker_desktop.Forms
{
    public partial class LinkLastFmForm : Form
    {
        private readonly string[] SORT_OPTIONS_TOP_ARTIST_ASC = ["Favorite (asc)", "Name (asc)", "PlayCount (asc)"];
        private readonly string[] SORT_OPTIONS_TOP_ARTIST_DESC = ["Favorite (desc)", "Name (desc)", "PlayCount (desc)"];

        private readonly string[] SORT_OPTIONS_RECENT_TRACK_ASC = ["Favorite (asc)", "Name (asc)", "Artist (asc)", "Album (asc)"];
        private readonly string[] SORT_OPTIONS_RECENT_TRACK_DESC = ["Favorite (desc)", "Name (desc)", "Artist (desc)", "Album (desc)"];

        private const string FILLED_STAR = "\u2605";
        private const string EMPTY_STAR = "\u2730";

        private DataTable _tableData = new DataTable();
        private Panel? _pnlSearchAndSort = null;
        private TextBox? _txtSearch = null;
        private Button _btnSort = new Button();
        private ContextMenuStrip? _sortMenu = null;

        private bool _lastFMSortVisible = false;
        private List<UserFavoriteMedia> _favorites = [];
        private List<LastFM_Artist> _topArtists = [];
        private int _topArtistCount = 0;
        private List<LastFM_Track> _recentTracks = [];
        private int _recentTrackCount = 0;

        private string _dataOption = "";

        public LinkLastFmForm(string option = "")
        {
            InitializeComponent();

            // Store the endpoint option.
            _dataOption = option;

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
                if (!string.IsNullOrEmpty(UserAppAccount.UserLastFmID) && !string.IsNullOrEmpty(_dataOption))
                {
                    await BuildViewGridBasedOnOption();

                    // Subscribe to event handlers:
                    // When any of the favorite buttons are clicked.
                    lastFmDataGridView.CellClick += btnFavorite_CellClick!;
                    // When any sort items in the sort menu are clicked.
                    _sortMenu.ItemClicked += sortMenu_ItemClicked!;
                    // When the sort menu button is clicked.
                    _btnSort.Click += btnSort_Click!;
                    // When user presses a button in search bar.
                    _txtSearch.KeyDown += txtSearch_KeyDown!;
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

        /// <summary>
        /// Builds display based on endpoint option.
        /// </summary>
        private async Task BuildViewGridBasedOnOption()
        {
            if (_pnlSearchAndSort == null)
            {
                BuildSearchAndSortPanel();
            }

            if (_dataOption == MainForm.LastFMOptions[0])
            {
                // Retrieve data.
                (bool success1, List<LastFM_Artist>? topArtists) = await LastFMApi.GetUserTopArtists();

                // Store the count for an error message that occurs when user doesn't have data.
                // Placed here because this is the freshest data.
                _topArtistCount = topArtists != null ? topArtists.Count : 0;

                // Save data.
                _topArtists = topArtists ?? [];

                if (_sortMenu == null)
                {
                    _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_TOP_ARTIST_ASC);
                }
                if (_txtSearch != null)
                {
                    _txtSearch.PlaceholderText = "Search for artist...";
                }

                // Build display.
                BuildTopArtistViewGrid(_topArtists);
            }
            else if (_dataOption == MainForm.LastFMOptions[1])
            {
                // Retrieve data.
                (bool success2, List<LastFM_Track>? recentTracks) = await LastFMApi.GetUserRecentTracks();

                // Store the count for an error message that occurs when user doesn't have data.
                // Placed here because this is the freshest data.
                _recentTrackCount = recentTracks != null ? recentTracks.Count : 0;

                // Save data.
                _recentTracks = recentTracks ?? [];

                if (_sortMenu == null)
                {
                    _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_RECENT_TRACK_ASC);
                }
                if (_txtSearch != null)
                {
                    _txtSearch.PlaceholderText = "Search for track...";
                }

                // Build display.
                BuildRecentTrackViewGrid(_recentTracks);
            }
        }

        private void BuildTopArtistViewGrid(List<LastFM_Artist> topArtists)
        {
            _tableData = new DataTable();

            // Columns:
            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Name");
            _tableData.Columns.Add("PlayCount");

            // Return if no top artist.
            if (_topArtistCount <= 0 || _topArtists == null)
            {
                MessageBox.Show("You don't have any top artists.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Foreach data,
            foreach (LastFM_Artist artist in topArtists)
            {
                // Add row.
                _tableData.Rows.Add(artist.Mbid, artist.Name, artist.PlayCount);
            }

            lastFmDataGridView.DataSource = _tableData;

            // Configurations:
            lastFmDataGridView.Columns["ID"].Visible = false;
            lastFmDataGridView.RowHeadersVisible = false;
            lastFmDataGridView.AllowUserToAddRows = false;
            lastFmDataGridView.ReadOnly = true;

            lastFmDataGridView.Columns["Name"].Width = 200;
            lastFmDataGridView.Columns["PlayCount"].Width = 200;

            BuildFavoriteButtonColumn();
        }

        private void BuildRecentTrackViewGrid(List<LastFM_Track> recentTracks)
        {
            _tableData = new DataTable();

            // Columns: 
            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Name");
            _tableData.Columns.Add("Artist");
            _tableData.Columns.Add("Album");

            // Return if no recent tracks.
            if (_recentTrackCount <= 0 || _recentTracks == null)
            {
                MessageBox.Show("You don't have any recent tracks.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Foreach data,
            foreach (LastFM_Track track in recentTracks)
            {
                // Add row.
                _tableData.Rows.Add(track.Name, track.Name, track.ArtistName, track.AlbumName);
            }

            lastFmDataGridView.DataSource = _tableData;

            // Configurations:
            lastFmDataGridView.Columns["ID"].Visible = false;
            lastFmDataGridView.RowHeadersVisible = false;
            lastFmDataGridView.AllowUserToAddRows = false;
            lastFmDataGridView.ReadOnly = true;

            lastFmDataGridView.Columns["Name"].Width = 200;
            lastFmDataGridView.Columns["Artist"].Width = 200;
            lastFmDataGridView.Columns["Album"].Width = 200;

            BuildFavoriteButtonColumn();
        }

        private async void BuildFavoriteButtonColumn()
        {
            AppElement.AddFavoriteButtonColumn(lastFmDataGridView);

            // Retrieve the list of user's favorite media.
            _favorites = await UserAppAccount.GetFavoriteMediaList();

            // Foreach row in the data grid,
            foreach (DataGridViewRow row in lastFmDataGridView.Rows)
            {
                // Default: Unfavorite button.
                row.Cells["btnFavorite"].Value = EMPTY_STAR;

                if (_dataOption == MainForm.LastFMOptions[0])
                {
                    // Retrieve the current artist name.
                    string currentRowArtistName = (string)row.Cells["Name"].Value;

                    // Retrieve the artist with the same name from the favorite list.
                    // Ensure that the record is of the Artist media type.
                    var favoriteArtist = _favorites.FirstOrDefault(a => a.Artist == currentRowArtistName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist);

                    // If there is an artist,
                    if (favoriteArtist != null)
                    {
                        // Favorite the button.
                        row.Cells["btnFavorite"].Value = FILLED_STAR;
                    }
                }
                else if (_dataOption == MainForm.LastFMOptions[1])
                {
                    // Retrieve the current track name.
                    string currentRowTrackName = (string)row.Cells["Name"].Value;

                    // Retrieve the track with the same name from the favorite list.
                    // Ensure that the record is of the Song media type.
                    var favoriteTrack = _favorites.FirstOrDefault(t => t.Title == currentRowTrackName && t.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song);

                    // If there is a track,
                    if (favoriteTrack != null)
                    {
                        // Favorite the button.
                        row.Cells["btnFavorite"].Value = FILLED_STAR;
                    }
                }
            }
        }

        // Event: When a favorite button is clicked.
        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != lastFmDataGridView.Columns["btnFavorite"].Index)
                {
                    // Build favorite column after sorting using column header.
                    if (e.RowIndex == -1)
                    {
                        BuildFavoriteButtonColumn();
                    }

                    return;
                }

                // Retrieve the clicked button.
                var currentButton = lastFmDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Update the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                if (_dataOption == MainForm.LastFMOptions[0])
                {
                    // Retrieve the artist name.
                    string currentRowArtistName = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Name"].Value;

                    // Retrieve the artist with the same name from the favorite list.
                    // Ensure that the record is of the Artist media type.
                    var favoriteArtist = _favorites.FirstOrDefault(a => a.Artist == currentRowArtistName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist);

                    // If there is no favorite artist,
                    if (favoriteArtist == null)
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
                else if (_dataOption == MainForm.LastFMOptions[1])
                {
                    // Retrieve the track and artist name.
                    string currentRowTrackName = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Name"].Value;
                    string currentRowArtistName = (string)lastFmDataGridView.Rows[e.RowIndex].Cells["Artist"].Value;

                    // Retrieve the track with the same name from the favorite list.
                    // Ensure that the record is of the Song media type.
                    var favoriteTrack = _favorites.FirstOrDefault(a => a.Title == currentRowTrackName && a.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song);

                    // If there is no favorite track,
                    if (favoriteTrack == null)
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

        // Method: Build the search and sort panel.
        private void BuildSearchAndSortPanel()
        {
            _pnlSearchAndSort = AppElement.GetSearchAndSortPanel();

            // Add to form.
            this.Controls.Add(_pnlSearchAndSort);

            _txtSearch = (TextBox)_pnlSearchAndSort.Controls["txtSearch"]!;

            _btnSort = (Button)_pnlSearchAndSort.Controls["btnSort"]!;
        }

        // Event: When user presses a button in the search textbox.
        private async void txtSearch_KeyDown (object sender, KeyEventArgs e)
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

            if (_dataOption == MainForm.LastFMOptions[0])
            {
                // Query options:
                // Ensure that the search is case insensitive.
                QueryOptions<LastFM_Artist> optionArtist = new QueryOptions<LastFM_Artist>
                {
                    Where = a => a.Name.ToLower().Contains(text)
                };

                // Retrieve data.
                List<LastFM_Artist> resultArtists = DataFunctions.Sort(_topArtists, optionArtist) ?? [];

                // Store data, so that the sort works on the searched data.
                _topArtists = resultArtists;

                BuildTopArtistViewGrid(resultArtists);
            }
            else if (_dataOption == MainForm.LastFMOptions[1])
            {
                // Query options:
                // Ensure that the search is case insensitive.
                QueryOptions<LastFM_Track> optionTrack = new QueryOptions<LastFM_Track>
                {
                    Where = t => t.Name.ToLower().Contains(text)
                };

                // Retrieve data.
                List<LastFM_Track> resultTracks = DataFunctions.Sort(_recentTracks, optionTrack) ?? [];

                // Store data, so that the sort works on the searched data.
                _recentTracks = resultTracks;

                BuildRecentTrackViewGrid(resultTracks);
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
            if (!_lastFMSortVisible)
            {
                // Open the menu.
                _sortMenu.Show(_btnSort, new Point(0, _btnSort.Height));

                // Menu is visible.
                _lastFMSortVisible = true;
            }
            // If the button is clicked to close the menu,
            else
            {
                // Close the menu.
                _sortMenu.Close();

                // Menu is not visible.
                _lastFMSortVisible = false;
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
            if (_dataOption == MainForm.LastFMOptions[0])
            {
                List<LastFM_Artist> sortedArtists = [];

                // Sorting Option 1
                if (option == SORT_OPTIONS_TOP_ARTIST_ASC[0])
                {
                    // Sort data.
                    sortedArtists = SortTopArtist("asc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_TOP_ARTIST_DESC[0];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
                else if (option == SORT_OPTIONS_TOP_ARTIST_DESC[0])
                {
                    // Sort data.
                    sortedArtists = SortTopArtist("desc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_TOP_ARTIST_ASC[0];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
                // Sorting Option 2
                else if (option == SORT_OPTIONS_TOP_ARTIST_ASC[1])
                {
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.Name,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedArtists = DataFunctions.Sort(_topArtists, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_TOP_ARTIST_DESC[1];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
                else if (option == SORT_OPTIONS_TOP_ARTIST_DESC[1])
                {
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.Name,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedArtists = DataFunctions.Sort(_topArtists, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_TOP_ARTIST_ASC[1];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
                // Sorting Option 3
                else if (option == SORT_OPTIONS_TOP_ARTIST_ASC[2])
                {
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.PlayCount,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedArtists = DataFunctions.Sort(_topArtists, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_TOP_ARTIST_DESC[2];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
                else if (option == SORT_OPTIONS_TOP_ARTIST_DESC[2])
                {
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.PlayCount,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedArtists = DataFunctions.Sort(_topArtists, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_TOP_ARTIST_ASC[2];

                    // Display.
                    BuildTopArtistViewGrid(sortedArtists);
                }
            }
            // Display Option 2
            else if (_dataOption == MainForm.LastFMOptions[1])
            {
                List<LastFM_Track> sortedTracks = [];

                // Sorting Option 1
                if (option == SORT_OPTIONS_RECENT_TRACK_ASC[0])
                {
                    // Sort data.
                    sortedTracks = SortRecentTrack("asc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_RECENT_TRACK_DESC[0];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                else if (option == SORT_OPTIONS_RECENT_TRACK_DESC[0])
                {
                    // Sort data.
                    sortedTracks = SortRecentTrack("desc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_RECENT_TRACK_ASC[0];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                // Sorting Option 2
                else if (option == SORT_OPTIONS_RECENT_TRACK_ASC[1])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_RECENT_TRACK_DESC[1];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                else if (option == SORT_OPTIONS_RECENT_TRACK_DESC[1])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_RECENT_TRACK_ASC[1];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                // Sorting Option 3
                else if (option == SORT_OPTIONS_RECENT_TRACK_ASC[2])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.ArtistName,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_RECENT_TRACK_DESC[2];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                else if (option == SORT_OPTIONS_RECENT_TRACK_DESC[2])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.ArtistName,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_RECENT_TRACK_ASC[2];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                // Sorting Option 4
                else if (option == SORT_OPTIONS_RECENT_TRACK_ASC[3])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.AlbumName,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[3].Text = SORT_OPTIONS_RECENT_TRACK_DESC[3];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
                else if (option == SORT_OPTIONS_RECENT_TRACK_DESC[3])
                {
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.AlbumName,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedTracks = DataFunctions.Sort(_recentTracks, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[3].Text = SORT_OPTIONS_RECENT_TRACK_ASC[3];

                    // Display.
                    BuildRecentTrackViewGrid(sortedTracks);
                }
            }
        }

        private List<LastFM_Artist> SortTopArtist(string orderByDirection)
        {
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            List<LastFM_Artist> favorites = [];
            List<LastFM_Artist> unfavorites = [];

            foreach (LastFM_Artist artist in _topArtists)
            {
                // If artist is in favorites,
                if (sortedFavorites.Any(f => f.Artist == artist.Name))
                {
                    // Add it.
                    favorites.Add(artist);
                }
                // Else,
                else
                {
                    // Add to unfavorite list.
                    unfavorites.Add(artist);
                }
            }

            return DataFunctions.SortFavorite(orderByDirection, favorites, unfavorites);
        }

        private List<LastFM_Track> SortRecentTrack(string orderByDirection)
        {
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            List<LastFM_Track> favorites = [];
            List<LastFM_Track> unfavorites = [];

            foreach (LastFM_Track track in _recentTracks)
            {
                // If track is in favorites,
                if (sortedFavorites.Any(f => f.Title == track.Name))
                {
                    // Add it.
                    favorites.Add(track);
                }
                // Else,
                else
                {
                    // Add to unfavorite list.
                    unfavorites.Add(track);
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
                    var (added, msg) = await Add3rdPartyIDFunction.AddThirdPartyId(
                        UserAppAccount.LastFMPlatformID,
                        username);

                    // If added,
                    if (added)
                    {
                        pnlLink.Visible = false;

                        MessageBox.Show("Account linked successfully!\n\nChoose an option from the Last.fm menu to display data.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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