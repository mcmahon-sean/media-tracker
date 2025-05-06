using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Supabase;
using media_tracker_desktop.Models;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.ApiModels;
using System.Data;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;

namespace media_tracker_desktop.Forms
{
    public partial class LinkSteamForm : Form
    {
        private readonly string[] SORT_OPTIONS_ASC = ["Name (asc)", "Hours Played (asc)", "Favorite (asc)"];
        private readonly string[] SORT_OPTIONS_DESC = ["Name (desc)", "Hours Played (desc)", "Favorite (desc)"];

        private DataTable _tableData = new DataTable();
        private Panel _pnlSearchAndSort = new Panel();
        private TextBox _txtSearch = new TextBox();
        private Button _btnSort = new Button();
        private ContextMenuStrip _sortMenu = new ContextMenuStrip();

        private bool _steamSortVisible = false;
        private List<UserFavoriteMedia> _favorites = [];
        private List<Steam_Game> _ownedGames = [];

        public LinkSteamForm()
        {
            InitializeComponent();
        }

        private async void LinkSteamForm_Load(object sender, EventArgs e)
        {
            // Can't await in the constructor.
            await LoadSteamAsync();
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
                        _ownedGames = games ?? [];

                        BuildViewGrid(games ?? []);

                        BuildSearchAndSortPanel();

                        // Subscribe to event handlers:
                        // When any of the favorite buttons are clicked.
                        steamDataGridView.CellClick += btnFavorite_CellClick!;
                        // When any sort items in the sort menu are clicked.
                        _sortMenu.ItemClicked += sortMenu_ItemClicked!;
                        // When the sort menu button is clicked.
                        _btnSort.Click += btnSort_Click!;
                        // When user presses a button in search bar.
                        _txtSearch.KeyDown += txtSearch_KeyDown!;
                    }
                    else
                    {
                        BuildViewGrid([]);
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
        private void BuildViewGrid(List<Steam_Game> games)
        {
            // Create a table to be used for the data grid.
            _tableData = new DataTable();

            // Add the columns to be displayed.
            _tableData.Columns.Add("AppID");
            _tableData.Columns.Add("Title");
            _tableData.Columns.Add("Hours Played");

            if (_ownedGames.Count <= 0 || _ownedGames == null)
            {
                steamDataGridView.DataSource = _tableData;
                MessageBox.Show("You don't own any games.");
                return;
            }

            // Add the rows to be displayed,
            foreach (Steam_Game game in games)
            {
                _tableData.Rows.Add(game.AppID, game.Name, game.PlaytimeForever / 60);
            }

            // Add the table.
            steamDataGridView.DataSource = _tableData;

            // This is to retrieve the game's ID from the row in which the favorite button is clicked.
            //hide the appid column and the row header
            steamDataGridView.Columns["AppID"].Visible = false;
            steamDataGridView.RowHeadersVisible = false;
            steamDataGridView.AllowUserToAddRows = false;

            //set width for title column
            steamDataGridView.Columns["Title"].Width = 200;
            steamDataGridView.Columns["Hours Played"].Width = 200;

            BuildFavoriteButtonColumn();
        }

        private async void BuildFavoriteButtonColumn()
        {
            AppElement.AddFavoriteButtonColumn(steamDataGridView);

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
        }

        // Event Handler for Favorite Buttons in the Button Column of the data grid view.
        private async void btnFavorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ignore clicks that are not the favorite buttons.
                if (e.RowIndex < 0 || e.ColumnIndex != steamDataGridView.Columns["btnFavorite"].Index)
                {
                    // Build favorite column after sorting using column header.
                    if (e.RowIndex == -1)
                    {
                        BuildFavoriteButtonColumn();
                    }
                    return;
                }

                // Retrieve the clicked button.
                var currentButton = steamDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Retrieve the current game ID.
                string currentRowAppID = (string)steamDataGridView.Rows[e.RowIndex].Cells["AppID"].Value;

                // Update the list of user's favorite media.
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
                    BuildViewGrid(_ownedGames);
                }
            }
        }

        // Method: Search for data.
        private void SearchData(string text)
        {
            text.ToLower();

            // Query options:
            // Ensure that the search is case insensitive.
            QueryOptions<Steam_Game> option = new QueryOptions<Steam_Game>
            {
                Where = t => t.Name.ToLower().Contains(text)
            };

            // Retrieve data.
            List<Steam_Game> resultGames = DataFunctions.Sort(_ownedGames, option) ?? [];

            // Display data.
            BuildViewGrid(resultGames);
        }

        // Event: When sort button is clicked.
        private void btnSort_Click(object sender, EventArgs e)
        {
            // Retrieve sort button.
            _btnSort = (Button)sender;

            // If the button is clicked to open the menu,
            if (!_steamSortVisible)
            {
                // Open the menu.
                _sortMenu.Show(_btnSort, new Point(0, _btnSort.Height));

                // Menu is visible.
                _steamSortVisible = true;
            }
            // If the button is clicked to close the menu,
            else
            {
                // Close the menu.
                _sortMenu.Close();

                // Menu is not visible.
                _steamSortVisible = false;
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
            List<Steam_Game>? sortedGames = [];

            // If there is data to be sorted,
            if (_ownedGames != null)
            {
                // First Sorting Option
                if (option == SORT_OPTIONS_ASC[0])
                {
                    QueryOptions<Steam_Game> optionGame = new QueryOptions<Steam_Game>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "asc"
                    };

                    sortedGames = DataFunctions.Sort(_ownedGames, optionGame);

                    _sortMenu.Items[0].Text = SORT_OPTIONS_DESC[0];
                }
                else if (option == SORT_OPTIONS_DESC[0])
                {
                    QueryOptions<Steam_Game> optionGame = new QueryOptions<Steam_Game>
                    {
                        OrderBy = t => t.Name,
                        OrderByDirection = "desc"
                    };

                    sortedGames = DataFunctions.Sort(_ownedGames, optionGame);

                    _sortMenu.Items[0].Text = SORT_OPTIONS_ASC[0];
                }
                // Second Sorting Option
                else if (option == SORT_OPTIONS_ASC[1])
                {
                    QueryOptions<Steam_Game> optionGame = new QueryOptions<Steam_Game>
                    {
                        OrderBy = t => t.PlaytimeForever,
                        OrderByDirection = "asc"
                    };

                    sortedGames = DataFunctions.Sort(_ownedGames, optionGame);

                    _sortMenu.Items[1].Text = SORT_OPTIONS_DESC[1];
                }
                else if (option == SORT_OPTIONS_DESC[1])
                {
                    QueryOptions<Steam_Game> optionGame = new QueryOptions<Steam_Game>
                    {
                        OrderBy = t => t.PlaytimeForever,
                        OrderByDirection = "desc"
                    };

                    sortedGames = DataFunctions.Sort(_ownedGames, optionGame);

                    _sortMenu.Items[1].Text = SORT_OPTIONS_ASC[1];
                }
                // Third Sorting Option
                else if (option == SORT_OPTIONS_ASC[2])
                {
                    List<Steam_Game> games = RetrieveSortedFavorites("asc");

                    // Update list.
                    sortedGames = games;

                    // Update the menu item.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_DESC[2];
                }
                else if (option == SORT_OPTIONS_DESC[2])
                {
                    List<Steam_Game> games = RetrieveSortedFavorites("desc");

                    // Update list.
                    sortedGames = games;

                    // Update the menu item.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_ASC[2];
                }

                // Build based on whether or not the list was updated.
                BuildViewGrid(sortedGames ?? []);
            }
        }

        // Method: Sorts the favorites.
        private List<Steam_Game> RetrieveSortedFavorites(string orderByDirection)
        {
            // Sort options based on passed orderByDirection.
            // Also, only games.
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Game,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            // Retrieve the sorted favorites.
            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            // Initialize lists.
            List<Steam_Game> favoriteGameList = [];
            List<Steam_Game> unfavoriteGameList = [];

            // Foreach game that is currently in display,
            foreach (Steam_Game game in _ownedGames)
            {
                // If that game is a favorite,
                if (_favorites.Any(f => f.MediaPlatID == game.AppID.ToString()))
                {
                    // Add it to the favorite list.
                    favoriteGameList.Add(game);
                }
                // Else,
                else
                {
                    // Add to the unfavorite list.
                    unfavoriteGameList.Add(game);
                }
            }

            // If the order is asc,
            if (orderByDirection == "asc")
            {
                // Add list together so that the favorite is first.
                favoriteGameList.AddRange(unfavoriteGameList);

                return favoriteGameList;
            }
            // If the order is desc,
            else if (orderByDirection == "desc")
            {
                // Add list together so that the favorite is last.
                unfavoriteGameList.AddRange(favoriteGameList);

                return unfavoriteGameList;
            }
            else
            {
                return [];
            }
        }

        private async void linkButton_Click(object sender, EventArgs e)
        {
            // Ensure user is logged in.
            if (!UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show("Please Sign-In first.", "Not Signed-In", MessageBoxButtons.OK, MessageBoxIcon.Information);
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