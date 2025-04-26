using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using Supabase;
using media_tracker_desktop.Models;

namespace media_tracker_desktop.Forms
{
    public partial class LinkSteamForm : Form
    {
        private readonly SteamService _steam = new SteamService();

        public LinkSteamForm()
        {
            InitializeComponent();
            var supaConn = new SupabaseConnection();
            UserAppAccount.ConnectToDB(supaConn.GetClient());
            if (!string.IsNullOrEmpty(UserAppAccount.UserSteamID))
                _ = LoadSteamAsync();
        }

        private async Task LoadSteamAsync()
        {
            pnlLink.Visible = false;
            steamDataGridView.DataSource = await _steam.GetOwnedGamesAsync(UserAppAccount.UserSteamID);
        }

        private async void linkButton_Click(object sender, EventArgs e)
        {
            var steamId = steamTextBox.Text.Trim();
            var games = await _steam.GetOwnedGamesAsync(steamId);
            if (games.Count > 0)
            {
                var (added, msg) = await UserAppAccount.AddThirdPartyId(
                    UserAppAccount.SteamPlatformID,
                    steamId
                );
                if (added)
                    await LoadSteamAsync();
                else
                    MessageBox.Show($"Link failed: {msg}");
            }
            else
            {
                MessageBox.Show("No games found for that Steam ID.");
            }
        }
    }
}