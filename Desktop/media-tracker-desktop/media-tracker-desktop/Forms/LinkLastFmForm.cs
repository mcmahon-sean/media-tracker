using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using media_tracker_desktop.Models.LastFM;
using Supabase;
using media_tracker_desktop.Models;

namespace media_tracker_desktop.Forms
{
    public partial class LinkLastFmForm : Form
    {
        private readonly LastFmService _lastFm = new LastFmService();

        public LinkLastFmForm()
        {
            InitializeComponent();
            var supaConn = new SupabaseConnection();
            UserAppAccount.ConnectToDB(supaConn.GetClient());
            if(!string.IsNullOrEmpty(UserAppAccount.UserLastFmID))
                _ = LoadLastFmAsync();
        }

        private async Task LoadLastFmAsync()
        {
            pnlLink.Visible = false;
            var u = UserAppAccount.UserLastFmID;
            lastFmDataGridView.DataSource = new List<LastFM_User> { await _lastFm.GetUserInfoAsync(u) };
        }

        private async void linkButton_Click(object sender, EventArgs e)
        {
            var username = lastFmTextBox.Text.Trim();
            try
            {
                var userInfo = await _lastFm.GetUserInfoAsync(username);
                if(userInfo != null)
                {
                    var (added,msg) = await UserAppAccount.AddThirdPartyId(
                        UserAppAccount.LastFMPlatformID,
                        username);
                    if(added) await LoadLastFmAsync(); else MessageBox.Show($"Link failed: {msg}");
                }
                else MessageBox.Show("No user found.");
            }
            catch(Exception ex){MessageBox.Show($"Error: {ex.Message}");}
        }
    }
}