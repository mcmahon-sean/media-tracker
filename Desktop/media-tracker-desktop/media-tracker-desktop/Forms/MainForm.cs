using System;
using System.Windows.Forms;
using media_tracker_desktop;

namespace media_tracker_desktop.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowHome();
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
                    FlatStyle = FlatStyle.Flat
                };
                btnLogin.Click += (s, e) => ShowSignin();

                var btnSignup = new Button
                {
                    Text = "Sign Up",
                    Location = new System.Drawing.Point(100, 100),
                    BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
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
                Text = $"Steam ID: {UserAppAccount.UserSteamID ?? "Not linked"}",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            var lastFm = new Label
            {
                Text = $"Last.FM ID: {UserAppAccount.UserLastFmID ?? "Not linked"}",
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(20, 90),
                AutoSize = true
            };
            var tmdb = new Label
            {
                Text = $"TMDB ID: {UserAppAccount.UserTmdbAccountID ?? "Not linked"}",
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
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += (s, e) => ShowPlatformEdit();

            var btnLogout = new Button
            {
                Text = "Logout",
                Location = new System.Drawing.Point(160, 160),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
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
            dashPanel.Controls.Add(btnLogout);
            pnlContent.Controls.Add(dashPanel);
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
                Location = new System.Drawing.Point(150, 18),
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
                Location = new System.Drawing.Point(150, 58),
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
            var txtTmdb = new TextBox
            {
                Name = "txtTmdb",
                Text = UserAppAccount.UserTmdbAccountID ?? "",
                Location = new System.Drawing.Point(150, 98),
                Width = 200
            };

            // Save button
            var btnSave = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(150, 140),
                BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += (s, e) =>
            {
                UserAppAccount.UserSteamID = txtSteam.Text.Trim();
                UserAppAccount.UserLastFmID = txtLastFm.Text.Trim();
                UserAppAccount.UserTmdbAccountID = txtTmdb.Text.Trim();
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
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => ShowDashboard();

            // Add controls
            editPanel.Controls.Add(lblSteam);
            editPanel.Controls.Add(txtSteam);
            editPanel.Controls.Add(lblLastFm);
            editPanel.Controls.Add(txtLastFm);
            editPanel.Controls.Add(lblTmdb);
            editPanel.Controls.Add(txtTmdb);
            editPanel.Controls.Add(btnSave);
            editPanel.Controls.Add(btnCancel);

            pnlContent.Controls.Add(editPanel);
        }

        private void btnHome_Click(object sender, EventArgs e) => ShowHome();

        private void btnLinkSteam_Click(object sender, EventArgs e)
        {
            var f = new LinkSteamForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
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
