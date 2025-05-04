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

namespace media_tracker_desktop.Forms
{
    public partial class LinkLastFmForm : Form
    {
        private List<UserFavoriteMedia> _favorites = [];
        private ContextMenuStrip _filterMenu;
        private bool _lastFMFilterVisible = false;
        private Button _btnFilter;
        private Panel _pnlSearchAndSort = new Panel();
        private DataTable _table;
        private List<LastFM_Artist> _topArtists = [];
        private List<LastFM_Track> _recentTracks = [];

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

                        // Store, important for sorting.
                        this._topArtists = topArtists;
                        this._recentTracks = recentTracks;

                        BuildViewGrid(topArtists, recentTracks);
                        BuildSearchAndSortPanel();
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
            _table = new DataTable();

            _table.Columns.Add("ID");//probably won't need this
            _table.Columns.Add("Top Type");
            _table.Columns.Add("Artist Name");
            _table.Columns.Add("Top Track");

            foreach (LastFM_Artist artist in topArtists)
            {
                _table.Rows.Add(artist.Mbid, "Top Artist",artist.Name, null);
            }
            foreach (LastFM_Track track in recentTracks)
            {
                _table.Rows.Add("", "Top Track", track.ArtistName, track.Name);
            }

            lastFmDataGridView.DataSource = _table;

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
            if (!lastFmDataGridView.Columns.Contains("btnFavorite"))
            {
                lastFmDataGridView.Columns.Add(favoriteButtons);
            }

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

        // Method: Build the search and sort panel.
        private void BuildSearchAndSortPanel()
        {
            _pnlSearchAndSort = new Panel();

            // Properties:
            _pnlSearchAndSort.Dock = DockStyle.Top;
            _pnlSearchAndSort.Size = new Size(669, 60);
            _pnlSearchAndSort.BackColor = Color.FromArgb(45, 45, 48);

            // Add to form.
            this.Controls.Add(_pnlSearchAndSort);

            // Add search bar and filter button.
            AddSearchBar(_pnlSearchAndSort);
            AddFilterButton(_pnlSearchAndSort);
        }

        // Method: Add search bar for the panel.
        private void AddSearchBar(Panel panel)
        {
            TextBox txtSearch = new TextBox();

            // Properties:
            txtSearch.Location = new Point(15, 15);
            txtSearch.PlaceholderText = "Search for artist or track...";
            txtSearch.Width = 350;

            // Add to panel.
            panel.Controls.Add(txtSearch);

            // Events:
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        // Event: When user presses a button in the search textbox.
        private void txtSearch_KeyDown (object sender, KeyEventArgs e)
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
                    BuildViewGrid(_topArtists, _recentTracks);
                }
            }
        }

        // Method: Search for data.
        private void SearchData(string text)
        {
            text.ToLower();

            // Query options:
            // Ensure that the search is case insensitive.
            QueryOptions<LastFM_Artist> optionArtist = new QueryOptions<LastFM_Artist>
            {
                Where = a => a.Name.ToLower().Contains(text)
            };

            QueryOptions<LastFM_Track> optionTrack = new QueryOptions<LastFM_Track>
            {
                Where = t => t.Name.ToLower().Contains(text) || t.ArtistName.ToLower().Contains(text)
            };

            // Retrieve data.
            List<LastFM_Artist> resultArtist = DataFunctions.Sort(_topArtists, optionArtist) ?? [];
            List<LastFM_Track> resultTrack = DataFunctions.Sort(_recentTracks, optionTrack) ?? [];

            // Display data.
            BuildViewGrid(resultArtist, resultTrack);
        }

        // Method: Add filter button.
        private void AddFilterButton(Panel panel)
        {
            _btnFilter = new Button();

            // Properties:
            _btnFilter.Location = new Point(400, 15);
            _btnFilter.Text = "Filter Menu";
            _btnFilter.AutoSize = true;
            _btnFilter.BackColor = Color.White;

            // Add to panel.
            panel.Controls.Add(_btnFilter);

            // Events:
            _btnFilter.Click += btnFilter_Click;

            // Add filter menu for the button.
            AddFilterMenu(_btnFilter);
        }

        // Event: When filter button is clicked.
        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Retrieve filter button.
            _btnFilter = (Button)sender;

            // If the button is clicked to open the menu,
            if (!_lastFMFilterVisible)
            {
                // Open the menu.
                _filterMenu.Show(_btnFilter, new Point(0, _btnFilter.Height));

                // Menu is visible.
                _lastFMFilterVisible = true;
            }
            // If the button is clicked to close the menu,
            else
            {
                // Close the menu.
                _filterMenu.Close();

                // Menu is not visible.
                _lastFMFilterVisible = false;
            }
        }

        // Method: Adds a context menu strip to button.
        private void AddFilterMenu(Button button)
        {
            _filterMenu = new ContextMenuStrip();

            // Options:
            ToolStripMenuItem artistFilter = new ToolStripMenuItem("Artist (asc)");
            ToolStripMenuItem trackFilter = new ToolStripMenuItem("Track (asc)");
            ToolStripMenuItem favoriteFilter = new ToolStripMenuItem("Favorite (asc)");

            // Add to button.
            _filterMenu.Items.AddRange(new ToolStripItem[] { artistFilter, trackFilter, favoriteFilter });

            // Events:
            _filterMenu.ItemClicked += filterMenu_ItemClicked;
        }

        // Event: When a filter item is clicked in the filter menu,
        private void filterMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Retrieve the filter item.
            string? option = e.ClickedItem?.Text;

            // If there is an item,
            if (option != null)
            {
                // Update the button to show the filter item selected.
                _btnFilter.Text = option;

                // Filter lastFM display.
                FilterData(option);
            }
        }

        // Method: Filters the data based on filter option.
        private void FilterData(string option)
        {
            List<LastFM_Artist>? sortedTopArtists = [];
            List<LastFM_Track>? sortedRecentTracks = [];

            // If there is data to be filtered,
            if (_topArtists != null && _recentTracks != null)
            {
                if (option == "Artist (asc)")
                {
                    // Filter options.
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.Name,
                        OrderByDirection = "asc"
                    };

                    // Update list.
                    sortedTopArtists = DataFunctions.Sort(_topArtists, options);
                    // Retain list.
                    sortedRecentTracks = _recentTracks;

                    // Update the menu item.
                    _filterMenu.Items[0].Text = "Artist (desc)";
                }
                else if (option == "Artist (desc)")
                {
                    // Filter options.
                    QueryOptions<LastFM_Artist> options = new QueryOptions<LastFM_Artist>
                    {
                        OrderBy = a => a.Name,
                        OrderByDirection = "desc"
                    };

                    // Update list.
                    sortedTopArtists = DataFunctions.Sort(_topArtists, options);
                    // Retain list.
                    sortedRecentTracks = _recentTracks;

                    // Update the menu item.
                    _filterMenu.Items[0].Text = "Artist (asc)";
                }
                else if (option == "Track (asc)")
                {
                    // Filter options.
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "asc"
                    };

                    // Update list.
                    sortedTopArtists = _topArtists;
                    // Retain list.
                    sortedRecentTracks = DataFunctions.Sort(_recentTracks, options);

                    // Update the menu item.
                    _filterMenu.Items[1].Text = "Track (desc)";
                }
                else if (option == "Track (desc)")
                {
                    // Filter options.
                    QueryOptions<LastFM_Track> options = new QueryOptions<LastFM_Track>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "desc"
                    };

                    // Update list.
                    sortedTopArtists = _topArtists;
                    // Retain list.
                    sortedRecentTracks = DataFunctions.Sort(_recentTracks, options);

                    // Update the menu item.
                    _filterMenu.Items[1].Text = "Track (asc)";
                }
                else if (option == "Favorite (asc)") {
                    (List<LastFM_Artist> favoriteArtistList, List<LastFM_Track> favoriteTrackList) = RetrieveSortedFavorites("asc");

                    // Update list.
                    sortedTopArtists = favoriteArtistList;
                    // Retain list.
                    sortedRecentTracks = favoriteTrackList;

                    // Update the menu item.
                    _filterMenu.Items[2].Text = "Favorite (desc)";
                }
                else if (option == "Favorite (desc)")
                {
                    (List<LastFM_Artist> favoriteArtistList, List<LastFM_Track> favoriteTrackList) = RetrieveSortedFavorites("desc");

                    // Update list.
                    sortedTopArtists = favoriteArtistList;
                    // Retain list.
                    sortedRecentTracks = favoriteTrackList;

                    // Update the menu item.
                    _filterMenu.Items[2].Text = "Favorite (asc)";
                }

                    // Build based on whether or not the list was updated.
                    BuildViewGrid(sortedTopArtists ?? [], sortedRecentTracks ?? []);
            }
        }

        // Method: Sorts the favorites.
        private (List<LastFM_Artist>, List<LastFM_Track>) RetrieveSortedFavorites(string orderByDirection)
        {
            // Filter options based on passed orderByDirection.
            // Also, only songs and artist favorites.
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Song || m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Artist,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            // Retrieve the sorted favorites.
            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            // Initialize lists.
            List<LastFM_Artist> favoriteArtistList = [];
            List<LastFM_Artist> unfavoriteArtistList = [];
            List<LastFM_Track> favoriteTrackList = [];
            List<LastFM_Track> unfavoriteTrackList = [];

            // Foreach artist that is currently in display,
            foreach (LastFM_Artist artist in _topArtists)
            {
                // If that artist is a favorite,
                if (_favorites.Any(f => f.Artist == artist.Name))
                {
                    // Add it to the favorite list.
                    favoriteArtistList.Add(artist);
                }
                // Else,
                else
                {
                    // Add to the unfavorite list.
                    unfavoriteArtistList.Add(artist);
                }
            }

            // Foreach track that is currently in display,
            foreach (LastFM_Track track in _recentTracks)
            {
                // If that track is a favorite,
                if (_favorites.Any(f => f.Title == track.Name))
                {
                    // Add it to the favorite list.
                    favoriteTrackList.Add(track);
                }
                // Else,
                else
                {
                    // Add to the unfavorite list.
                    unfavoriteTrackList.Add(track);
                }
            }

            // If the order is asc,
            if (orderByDirection == "asc")
            {
                // Add list together so that the favorite is first.
                favoriteArtistList.AddRange(unfavoriteArtistList);
                favoriteTrackList.AddRange(unfavoriteTrackList);

                return (favoriteArtistList, favoriteTrackList);
            }
            // If the order is desc,
            else if (orderByDirection == "desc")
            {
                // Add list together so that the favorite is last.
                unfavoriteArtistList.AddRange(favoriteArtistList);
                unfavoriteTrackList.AddRange(favoriteTrackList);

                return (unfavoriteArtistList, unfavoriteTrackList);
            }
            else
            {
                return ([], []);
            }
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