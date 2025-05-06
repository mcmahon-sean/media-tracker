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

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeApp();
            ShowHome();
        }

        // Method: Initialize the components that are necessary for the app to run.
        private async void InitializeApp()
        {
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
                    BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat,
                    AutoSize = true
                };
                btnLogin.Click += (s, e) => ShowSignin();

                var btnSignup = new Button
                {
                    Text = "Sign Up",
                    Location = new System.Drawing.Point(100, 100),
                    BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat,
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
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnEdit.Click += (s, e) => ShowPlatformEdit();

            Button btnEditUser = new Button
            {
                Text = "Edit User",
                Location = new System.Drawing.Point(200, 160),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnEditUser.Click += btnEditUser_Click;

            Button btnDeleteUser = new Button
            {
                Text = "Delete User",
                Location = new System.Drawing.Point(305, 160),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnDeleteUser.Click += btnDeleteUser_Click;

            var btnLogout = new Button
            {
                Text = "Logout",
                Location = new System.Drawing.Point(430, 160),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
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
            var txtSteam = new TextBox
            {
                Name = "txtSteam",
                Text = UserAppAccount.UserSteamID ?? "",
                Location = new System.Drawing.Point(200, 18),
                Width = 200
            };

            // Last.fm
            var lblLastFm = new Label
            {
                Text = "Last.FM Username:",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            var txtLastFm = new TextBox
            {
                Name = "txtLastFm",
                Text = UserAppAccount.UserLastFmID ?? "",
                Location = new System.Drawing.Point(200, 58),
                Width = 200
            };

            // TMDB
            var lblTmdb = new Label
            {
                Text = "TMDB Account ID:",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 100),
                AutoSize = true
            };
            var btnTmdb = new Button
            {
                Name = "btnTmdb",
                Text = "Update TMDB",
                Location = new System.Drawing.Point(200, 98),
                Width = 200,
                BackColor = Color.White,
                AutoSize = true
            };

            btnTmdb.Click += btnTmdb_Click;

            // Save button
            var btnSave = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(150, 140),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnSave.Click += (s, e) =>
            {
                string newSteamID = txtSteam.Text.Trim();
                string newLastFmID = txtLastFm.Text.Trim();
                string? newTmdbID = _newSessionIDFromTMDBEditButton;

                UpdateUserPlatformIDs(newSteamID, newLastFmID, newTmdbID!);

                MessageBox.Show("Platform info updated!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowDashboard();
            };

            // Cancel button
            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(230, 140),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            btnCancel.Click += (s, e) => ShowDashboard();

            // Add controls
            editPanel.Controls.Add(lblSteam);
            editPanel.Controls.Add(txtSteam);
            editPanel.Controls.Add(lblLastFm);
            editPanel.Controls.Add(txtLastFm);
            editPanel.Controls.Add(lblTmdb);
            editPanel.Controls.Add(btnTmdb);
            editPanel.Controls.Add(btnSave);
            editPanel.Controls.Add(btnCancel);

            pnlContent.Controls.Add(editPanel);
        }

        private async void btnTmdb_Click(object sender, EventArgs e)
        {
            _newSessionIDFromTMDBEditButton = await TmdbApi.RetrieveSessionID();
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
            var f = new LinkSteamForm() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            pnlContent.Controls.Clear(); pnlContent.Controls.Add(f); f.Show(); lblTitle.Text = "Steam";
        }
        private void btnLinkLastFM_Click(object sender, EventArgs e)
        {
            var f = new LinkLastFmForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            pnlContent.Controls.Clear(); pnlContent.Controls.Add(f); f.Show(); lblTitle.Text = "Last.fm";
        }
        private void btnLinkTmdb_Click(object sender, EventArgs e)
        {
            var f = new LinkTmdbForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            pnlContent.Controls.Clear(); pnlContent.Controls.Add(f); f.Show(); lblTitle.Text = "TMDB";
        }
    }
}
