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
using media_tracker_desktop.Models.LastFM;
using media_tracker_desktop.Models.SupabaseFunctionObjects;

namespace media_tracker_desktop.Forms
{
    public partial class LinkSteamForm : Form
    {
        private readonly string[] SORT_OPTIONS_OWNED_GAME_ASC = ["Favorite (asc)", "Name (asc)", "Hours Played (asc)"];
        private readonly string[] SORT_OPTIONS_OWNED_GAME_DESC = ["Favorite (desc)", "Name (desc)", "Hours Played (desc)"];

        private const string FILLED_STAR = "\u2605";
        private const string EMPTY_STAR = "\u2730";

        private DataTable _tableData = new DataTable();
        private Panel? _pnlSearchAndSort = null;
        private TextBox? _txtSearch = null;
        private Button _btnSort = new Button();
        private ContextMenuStrip? _sortMenu = null;

        private bool _steamSortVisible = false;
        private List<UserFavoriteMedia> _favorites = [];
        private List<Steam_Game> _ownedGames = [];
        private int _ownedGameCount = 0;

        private string _dataOption = "";

        public LinkSteamForm(string option = "")
        {
            InitializeComponent();

            _dataOption = option;
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
                    await BuildViewGridBasedOnOption();                        

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

        /// <summary>
        /// Builds display based on endpoint option.
        /// </summary>
        private async Task BuildViewGridBasedOnOption()
        {
            if (_pnlSearchAndSort == null)
            {
                BuildSearchAndSortPanel();
            }

            if (_dataOption == MainForm.SteamOptions[0])
            {
                // Retrieve data.
                (bool success, List<Steam_Game>? ownedGames) = await SteamApi.GetOwnedGames();

                // Store the count for an error message that occurs when user doesn't have data.
                // Placed here because this is the freshest data.
                _ownedGameCount = ownedGames != null ? ownedGames.Count : 0;

                // Save data.
                _ownedGames = ownedGames ?? [];

                if (_sortMenu == null)
                {
                    _sortMenu = AppElement.GetSortMenu(SORT_OPTIONS_OWNED_GAME_ASC);
                }

                // Build display.
                BuildOwnedGameViewGrid(_ownedGames);
            }
        }

        private void BuildOwnedGameViewGrid(List<Steam_Game> ownedGames)
        {
            _tableData = new DataTable();

            // Columns:
            _tableData.Columns.Add("ID");
            _tableData.Columns.Add("Name");
            _tableData.Columns.Add("Hours Played");

            // Return if no owned games.
            if (_ownedGameCount <= 0 || _ownedGames == null)
            {
                MessageBox.Show("You don't own any game.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Foreach data,
            foreach (Steam_Game game in ownedGames)
            {
                // Add row.
                _tableData.Rows.Add(game.AppID, game.Name, game.PlaytimeForever);
            }

            steamDataGridView.DataSource = _tableData;

            // Configurations:
            steamDataGridView.Columns["ID"].Visible = false;
            steamDataGridView.RowHeadersVisible = false;
            steamDataGridView.AllowUserToAddRows = false;
            steamDataGridView.ReadOnly = true;

            steamDataGridView.Columns["Name"].Width = 200;
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
                // Default: Unfavorite the button.
                row.Cells["btnFavorite"].Value = EMPTY_STAR;

                if (_dataOption == MainForm.SteamOptions[0])
                {
                    // Retrieve the current game ID.
                    string currentRowAppID = (string)row.Cells["ID"].Value;

                    // Retrieve the game with the same game ID from the favorite list.
                    // Make sure that it is from the Steam platform.
                    var favoriteGame = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowAppID && g.PlatformID == UserAppAccount.SteamPlatformID);

                    // If a game exist, mark the button as favorite.
                    if (favoriteGame != null)
                    {
                        row.Cells["btnFavorite"].Value = FILLED_STAR;
                    }
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

                // Update the list of user's favorite media.
                _favorites = await UserAppAccount.GetFavoriteMediaList();

                if (_dataOption == MainForm.SteamOptions[0])
                {
                    // Retrieve the current game ID.
                    string currentRowID = (string)steamDataGridView.Rows[e.RowIndex].Cells["ID"].Value;

                    // Retrieve the game with the same game ID from the favorite list.
                    // Make sure that it is from the Steam platform.
                    var favoriteGame = _favorites.FirstOrDefault(g => g.MediaPlatID == currentRowID && g.PlatformID == UserAppAccount.SteamPlatformID);

                    // If there is no game,
                    if (favoriteGame == null)
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

                    // Retrieve the game title of the current row.
                    string currentRowName = (string)steamDataGridView.Rows[e.RowIndex].Cells["Name"].Value;

                    // Update the favorite status of the media.
                    // The SP unfavorites/favorites media.
                    await UserAppAccount.FavoriteMedia(
                        platformID: UserAppAccount.SteamPlatformID,
                        username: UserAppAccount.Username,
                        mediaTypeID: UserAppAccount.MediaTypeID.Game,
                        mediaPlatformID: currentRowID,
                        title: currentRowName
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
            _txtSearch.PlaceholderText = "Search for game name...";

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

            if (_dataOption == MainForm.SteamOptions[0])
            {
                // Query options:
                // Ensure that the search is case insensitive.
                QueryOptions<Steam_Game> option = new QueryOptions<Steam_Game>
                {
                    Where = g => g.Name.ToLower().Contains(text)
                };

                // Retrieve data.
                List<Steam_Game> resultGames = DataFunctions.Sort(_ownedGames, option) ?? [];

                // Store data, so that the sort works on the searched data.
                _ownedGames = resultGames;

                BuildOwnedGameViewGrid(resultGames);
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
            // Display Option 1
            if (_dataOption == MainForm.SteamOptions[0])
            {
                List<Steam_Game> sortedGames = [];

                // Sorting Option 1
                if (option == SORT_OPTIONS_OWNED_GAME_ASC[0])
                {
                    // Sort data.
                    sortedGames = SortOwnedGame("asc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_OWNED_GAME_DESC[0];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
                else if (option == SORT_OPTIONS_OWNED_GAME_DESC[0])
                {
                    // Sort data.
                    sortedGames = SortOwnedGame("desc");

                    // Change sort option to opposite.
                    _sortMenu.Items[0].Text = SORT_OPTIONS_OWNED_GAME_ASC[0];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
                // Sorting Option 2
                if (option == SORT_OPTIONS_OWNED_GAME_ASC[1])
                {
                    QueryOptions<Steam_Game> options = new QueryOptions<Steam_Game>
                    {
                        OrderBy = g => g.Name,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedGames = DataFunctions.Sort(_ownedGames, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_OWNED_GAME_DESC[1];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
                else if (option == SORT_OPTIONS_OWNED_GAME_DESC[1])
                {
                    QueryOptions<Steam_Game> options = new QueryOptions<Steam_Game>
                    {
                        OrderBy = g => g.Name,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedGames = DataFunctions.Sort(_ownedGames, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[1].Text = SORT_OPTIONS_OWNED_GAME_ASC[1];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
                // Sorting Option 3
                if (option == SORT_OPTIONS_OWNED_GAME_ASC[2])
                {
                    QueryOptions<Steam_Game> options = new QueryOptions<Steam_Game>
                    {
                        OrderBy = g => g.PlaytimeForever,
                        OrderByDirection = "asc"
                    };

                    // Sort data.
                    sortedGames = DataFunctions.Sort(_ownedGames, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_OWNED_GAME_DESC[2];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
                else if (option == SORT_OPTIONS_OWNED_GAME_DESC[2])
                {
                    QueryOptions<Steam_Game> options = new QueryOptions<Steam_Game>
                    {
                        OrderBy = g => g.PlaytimeForever,
                        OrderByDirection = "desc"
                    };

                    // Sort data.
                    sortedGames = DataFunctions.Sort(_ownedGames, options) ?? [];

                    // Change sort option to opposite.
                    _sortMenu.Items[2].Text = SORT_OPTIONS_OWNED_GAME_ASC[2];

                    // Display.
                    BuildOwnedGameViewGrid(sortedGames);
                }
            }
        }

        private List<Steam_Game> SortOwnedGame(string orderByDirection)
        {
            // Sort options based on passed orderByDirection.
            // Also, only games.
            QueryOptions<UserFavoriteMedia> options = new QueryOptions<UserFavoriteMedia>
            {
                Where = m => m.MediaTypeID == (int)UserAppAccount.MediaTypeID.Game,
                OrderBy = m => m.MediaID,
                OrderByDirection = orderByDirection
            };

            List<UserFavoriteMedia> sortedFavorites = DataFunctions.Sort(_favorites, options) ?? [];

            List<Steam_Game> favorites = [];
            List<Steam_Game> unfavorites = [];

            foreach (Steam_Game game in _ownedGames)
            {
                // If game is in favorites,
                if (sortedFavorites.Any(f => f.MediaPlatID == game.AppID.ToString()))
                {
                    // Add it.
                    favorites.Add(game);
                }
                // Else,
                else
                {
                    // Add to unfavorite list.
                    unfavorites.Add(game);
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
                    var (added, msg) = await Add3rdPartyIDFunction.AddThirdPartyId(
                        UserAppAccount.SteamPlatformID,
                        steamId
                    );

                    // If added,
                    if (added)
                    {
                        pnlLink.Visible = false;

                        MessageBox.Show("Account linked successfully!\n\nChoose an option from the Steam menu to display data.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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