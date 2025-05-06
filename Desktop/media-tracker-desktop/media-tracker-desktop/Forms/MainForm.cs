using System;
using System.Windows.Forms;
using media_tracker_desktop;
using media_tracker_desktop.Models.ApiModels;

namespace media_tracker_desktop.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private string? _newSessionIDFromTMDBEditButton = string.Empty;

        private TextBox txtSteam = new TextBox();
        private TextBox txtLastFm = new TextBox();

        private bool isSteamToBeUnlinked = false;
        private bool isLastFMToBeUnlinked = false;
        private bool isTMDBToBeUnlinked = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeApp();
            ShowHome();
        }

        // Method: Initialize the components that are necessary for the app to run.
        private async void InitializeApp()
        {
            // Initialize Icons for buttons.
            AppElement.AddMainIcon(btnLinkLastFM, Properties.Resources.icon_music, new Size(20, 20));
            AppElement.AddMainIcon(btnLinkSteam, Properties.Resources.icon_games, new Size(30, 18));
            AppElement.AddMainIcon(btnLinkTmdb, Properties.Resources.icon_movies, new Size(30, 18));

            // Initialize the database.
            string message = await SupabaseConnection.InitializeDB();

            // If success,
            if (message == "success")
            {
                // Connect the user app account to the DB.
                UserAppAccount.ConnectToDB(SupabaseConnection.GetClient());

                // Initialize the Api Models.
                LastFMApi.Initialize();
                SteamApi.Initialize();
                TmdbApi.Initialize();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void ShowHome()
        {
            pnlContent.Controls.Clear();

            if (string.IsNullOrEmpty(UserAppAccount.Username))
            {
                lblTitle.Text = "Media Tracker | Home";

                var homePanel = new Panel { Dock = DockStyle.Fill };
                var notSignedIn = new Label
                {
                    Text = "Not Signed In",
                    ForeColor = System.Drawing.Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 20)
                };
                var signInPrompt = new Label
                {
                    Text = "Sign in to use the Media Tracker",
                    ForeColor = System.Drawing.Color.Gray,
                    Font = new System.Drawing.Font("Segoe UI", 10F),
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 60)
                };
                var btnLogin = new Button
                {
                    Text = "Login",
                    Location = new System.Drawing.Point(20, 100),
                    BackColor = System.Drawing.Color.White,
                    ForeColor = System.Drawing.Color.Black,
                    FlatStyle = FlatStyle.Popup,
                    AutoSize = true
                };
                btnLogin.Click += (s, e) => ShowSignin();

                var btnSignup = new Button
                {
                    Text = "Sign Up",
                    Location = new System.Drawing.Point(100, 100),
                    BackColor = System.Drawing.Color.White,
                    ForeColor = System.Drawing.Color.Black,
                    FlatStyle = FlatStyle.Popup,
                    AutoSize = true
                };
                btnSignup.Click += (s, e) => ShowSignup();

                homePanel.Controls.Add(notSignedIn);
                homePanel.Controls.Add(signInPrompt);
                homePanel.Controls.Add(btnLogin);
                homePanel.Controls.Add(btnSignup);
                pnlContent.Controls.Add(homePanel);
            }
            else
            {
                ShowDashboard();
            }
        }

        private void ShowSignin()
        {
            var signin = new SigninForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            signin.LoginSucceeded += (s, e) => ShowDashboard();
            signin.SwitchToRegister += (s, e) => ShowSignup();

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(signin);
            signin.Show();
            lblTitle.Text = "Media Tracker | Login";
        }

        private void ShowSignup()
        {
            var signup = new SignupForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            signup.RegistrationSucceeded += (s, e) => ShowSignin();

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(signup);
            signup.Show();
            lblTitle.Text = "Media Tracker | Sign Up";
        }

        private void ShowDashboard()
        {
            pnlContent.Controls.Clear();
            lblTitle.Text = "Media Tracker | Home";

            string steamIdText = string.IsNullOrEmpty(UserAppAccount.UserSteamID) ? "Not Linked" : UserAppAccount.UserSteamID;

            string lastFmIdText = string.IsNullOrEmpty(UserAppAccount.UserLastFmID) ? "Not Linked" : UserAppAccount.UserLastFmID;

            string tmdbIdText = string.IsNullOrEmpty(UserAppAccount.UserTmdbAccountID) ? "Not Linked" : UserAppAccount.UserTmdbAccountID;

            var dashPanel = new Panel { Dock = DockStyle.Fill };

            var greeting = new Label
            {
                Text = $"Hello, {UserAppAccount.Username}!",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };
            var steamId = new Label
            {
                Text = $"Steam ID: {steamIdText}",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            var lastFm = new Label
            {
                Text = $"Last.FM ID: {lastFmIdText}",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 90),
                AutoSize = true
            };
            var tmdb = new Label
            {
                Text = $"TMDB ID: {tmdbIdText}",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 120),
                AutoSize = true
            };

            var btnEdit = new Button
            {
                Text = "Add/Edit Platform",
                Location = new System.Drawing.Point(20, 160),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnEdit.Click += (s, e) => ShowPlatformEdit();

            Button btnEditUser = new Button
            {
                Text = "Edit User",
                Location = new System.Drawing.Point(200, 160),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnEditUser.Click += btnEditUser_Click!;

            Button btnDeleteUser = new Button
            {
                Text = "Delete User",
                Location = new System.Drawing.Point(305, 160),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnDeleteUser.Click += btnDeleteUser_Click!;

            var btnLogout = new Button
            {
                Text = "Logout",
                Location = new System.Drawing.Point(430, 160),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnLogout.Click += (s, e) => {
                UserAppAccount.LogOut();
                ShowHome();
            };

            dashPanel.Controls.Add(greeting);
            dashPanel.Controls.Add(steamId);
            dashPanel.Controls.Add(lastFm);
            dashPanel.Controls.Add(tmdb);
            dashPanel.Controls.Add(btnEdit);
            dashPanel.Controls.Add(btnEditUser);
            dashPanel.Controls.Add(btnDeleteUser);
            dashPanel.Controls.Add(btnLogout);
            pnlContent.Controls.Add(dashPanel);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            this.Hide();

            UpdateUserForm editForm = new UpdateUserForm();

            editForm.LaunchEditUserForm();

            this.Show();
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DialogResult confirmation = MessageBox.Show("Are you sure you want to delete this account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            // If user confirms deletion,
            if (confirmation == DialogResult.Yes)
            {
                // Delete user.
                Dictionary<string, dynamic> result = await UserAppAccount.DeleteUser();

                MessageBox.Show(result["statusMessage"]);

                // If user successfully deleted,
                if (result["status"] == "success")
                {
                    // Show home screen with the login and signup buttons.
                    ShowHome();
                }
            }
        }

        private void ShowPlatformEdit()
        {
            pnlContent.Controls.Clear();
            lblTitle.Text = "Edit Linked Accounts";

            var editPanel = new Panel { Dock = DockStyle.Fill };

            // Steam
            var lblSteam = new Label
            {
                Text = "Steam ID:",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            txtSteam = new TextBox
            {
                Name = "txtSteam",
                Text = UserAppAccount.UserSteamID ?? "",
                Location = new System.Drawing.Point(200, 18),
                Width = 200
            };
            Button btnUnlinkSteam = new Button
            {
                Text = "Unlink",
                Location = new System.Drawing.Point(410, 18),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnUnlinkSteam.Click += btnUnlinkSteam_Click!;

            // Last.fm
            var lblLastFm = new Label
            {
                Text = "Last.FM Username:",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            txtLastFm = new TextBox
            {
                Name = "txtLastFm",
                Text = UserAppAccount.UserLastFmID ?? "",
                Location = new System.Drawing.Point(200, 58),
                Width = 200
            };
            Button btnUnlinkLastFM = new Button
            {
                Text = "Unlink",
                Location = new System.Drawing.Point(410, 58),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnUnlinkLastFM.Click += btnUnlinkLastFM_Click!;

            // TMDB
            var lblTmdb = new Label
            {
                Text = "TMDB Account ID:",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 100),
                AutoSize = true
            };
            Button btnTmdb = new Button
            {
                Name = "btnTmdb",
                Text = "Update TMDB",
                Location = new System.Drawing.Point(200, 98),
                Width = 200,
                BackColor = Color.White,
                AutoSize = true
            };
            btnTmdb.Click += btnTmdb_Click!;

            Button btnUnlinkTMDB = new Button
            {
                Text = "Unlink",
                Location = new System.Drawing.Point(410, 98),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnUnlinkTMDB.Click += btnUnlinkTMDB_Click!;


            // Save button
            var btnSave = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(150, 140),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnSave.Click += btnSave_Click!;

            // Cancel button
            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(230, 140),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnCancel.Click += (s, e) => ShowDashboard();

            // Add controls
            editPanel.Controls.Add(lblSteam);
            editPanel.Controls.Add(txtSteam);
            editPanel.Controls.Add(btnUnlinkSteam);
            editPanel.Controls.Add(lblLastFm);
            editPanel.Controls.Add(txtLastFm);
            editPanel.Controls.Add(btnUnlinkLastFM);
            editPanel.Controls.Add(lblTmdb);
            editPanel.Controls.Add(btnTmdb);
            editPanel.Controls.Add(btnUnlinkTMDB);
            editPanel.Controls.Add(btnSave);
            editPanel.Controls.Add(btnCancel);

            pnlContent.Controls.Add(editPanel);
        }

        private async void btnTmdb_Click(object sender, EventArgs e)
        {
            try
            {
                _newSessionIDFromTMDBEditButton = await TmdbApi.RetrieveSessionID();
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string message = "";

            string newSteamID = txtSteam.Text.Trim();
            string newLastFmID = txtLastFm.Text.Trim();
            string? newTmdbID = _newSessionIDFromTMDBEditButton;

            UpdateUserPlatformIDs(newSteamID, newLastFmID, newTmdbID!);

            if (!string.IsNullOrEmpty(newSteamID) || !string.IsNullOrEmpty(newLastFmID) || !string.IsNullOrEmpty(newTmdbID))
            {
                message += "Platform info updated!\n\n";
            }

            if (isSteamToBeUnlinked)
            {
                // Unlink account.
                Dictionary<string, dynamic> result = await UserAppAccount.UnlinkApiAccount(UserAppAccount.SteamPlatformID);

                message += result["statusMessage"] + "\n";

                if (result["status"] == "success")
                {
                    txtSteam.Text = "";
                }
            }

            if (isLastFMToBeUnlinked)
            {
                // Unlink account.
                Dictionary<string, dynamic> result = await UserAppAccount.UnlinkApiAccount(UserAppAccount.LastFMPlatformID);

                message += result["statusMessage"] + "\n";

                if (result["status"] == "success")
                {
                    txtLastFm.Text = "";
                }
            }

            if (isTMDBToBeUnlinked)
            {
                // Unlink account.
                Dictionary<string, dynamic> result = await UserAppAccount.UnlinkApiAccount(UserAppAccount.TMDBPlatformID);

                message += result["statusMessage"] + "\n";
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ShowDashboard();
        }

        private void btnUnlinkSteam_Click(object sender, EventArgs e)
        {
            isSteamToBeUnlinked = false;

            // If user has an account linked,
            if (!string.IsNullOrEmpty(UserAppAccount.UserSteamID))
            {
                DialogResult confirmation = MessageBox.Show("Are you sure you want to unlink your steam account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // If user confirms unlink,
                if (confirmation == DialogResult.Yes)
                {
                    // Mark it to be unlinked.
                    isSteamToBeUnlinked = true;
                }
            }
            else
            {
                MessageBox.Show("You don't have a steam account linked.");
            }
        }

        private void btnUnlinkLastFM_Click(object sender, EventArgs e)
        {
            isLastFMToBeUnlinked = false;

            // If user has an account linked,
            if (!string.IsNullOrEmpty(UserAppAccount.UserLastFmID))
            {
                DialogResult confirmation = MessageBox.Show("Are you sure you want to unlink your lastFM account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // If user confirms unlink,
                if (confirmation == DialogResult.Yes)
                {
                    // Mark it to be unlinked.
                    isLastFMToBeUnlinked = true;
                }
            }
            else
            {
                MessageBox.Show("You don't have a lastFM account linked.");
            }
        }

        private void btnUnlinkTMDB_Click(object sender, EventArgs e)
        {
            isTMDBToBeUnlinked = false;

            // If user has an account linked,
            if (!string.IsNullOrEmpty(UserAppAccount.UserTmdbSessionID))
            {
                DialogResult confirmation = MessageBox.Show("Are you sure you want to unlink your TMDB account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // If user confirms unlink,
                if (confirmation == DialogResult.Yes)
                {
                    // Mark it to be unlinked.
                    isTMDBToBeUnlinked = true;

                    // Erase session ID in case user linked tmdb then clicked unlink tmdb.
                    _newSessionIDFromTMDBEditButton = "";
                }
            }
            else
            {
                MessageBox.Show("You don't have a TMDB account linked.");
            }
        }

        private async void UpdateUserPlatformIDs(string newSteamID, string newLastFmID, string newTmdbID)
        {
            if (!string.IsNullOrEmpty(newSteamID))
            {
                (bool success, string message) = await UserAppAccount.UpdateUserPlatformID(UserAppAccount.SteamPlatformID, newSteamID);
            }

            if (!string.IsNullOrEmpty(newLastFmID))
            {
                (bool success, string message) = await UserAppAccount.UpdateUserPlatformID(UserAppAccount.LastFMPlatformID, newLastFmID);
            }

            if (!string.IsNullOrEmpty(newTmdbID))
            {
                (bool success, string message) = await UserAppAccount.UpdateUserPlatformID(UserAppAccount.TMDBPlatformID, newTmdbID);
            }
        }

        private void btnHome_Click(object sender, EventArgs e) => ShowHome();

        private void btnLinkSteam_Click(object sender, EventArgs e)
        {
            var f = new LinkSteamForm() { 
                TopLevel = false, 
                FormBorderStyle = FormBorderStyle.None, 
                Dock = DockStyle.Fill 
            };

            pnlContent.Controls.Clear(); 
            pnlContent.Controls.Add(f); 
            f.Show(); 
            lblTitle.Text = "Steam";
        }
        private void btnLinkLastFM_Click(object sender, EventArgs e)
        {
            var f = new LinkLastFmForm { 
                TopLevel = false, 
                FormBorderStyle = FormBorderStyle.None, 
                Dock = DockStyle.Fill 
            };

            pnlContent.Controls.Clear(); 
            pnlContent.Controls.Add(f); 
            f.Show(); 
            lblTitle.Text = "Last.fm";
        }
        private void btnLinkTmdb_Click(object sender, EventArgs e)
        {
            var f = new LinkTmdbForm { 
                TopLevel = false, 
                FormBorderStyle = FormBorderStyle.None, 
                Dock = DockStyle.Fill 
            };

            pnlContent.Controls.Clear(); 
            pnlContent.Controls.Add(f); 
            f.Show(); 
            lblTitle.Text = "TMDB";
        }
    }
}
