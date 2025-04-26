using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using media_tracker_desktop.Services;
using media_tracker_desktop.Models.TMDB;
using Supabase;
using media_tracker_desktop.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;

namespace media_tracker_desktop.Forms
{
    public partial class LinkTmdbForm : Form
    {
        private readonly TmdbService _tmdbSvc = new TmdbService();

        public LinkTmdbForm()
        {
            InitializeComponent();
            var supaConn = new SupabaseConnection();
            UserAppAccount.ConnectToDB(supaConn.GetClient());
            if(!string.IsNullOrEmpty(UserAppAccount.UserTmdbSessionID))
                _ = LoadTmdbAsync();
        }

        private async Task LoadTmdbAsync()
        {
            pnlLink.Visible = false;
            var session = UserAppAccount.UserTmdbSessionID;
            var acct = await _tmdbSvc.GetAccountDetailsAsync(session);
            tmdbDataGridView.DataSource = new List<TMDB_Account> { acct };
        }

        private async void linkButton_Click(object sender, EventArgs e)
        {
            try
            {
                string authToken = ConfigurationManager.AppSettings["TMDBNathanAuthToken"];
                var client  = new RestClient("https://api.themoviedb.org/3/authentication/token/new");
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {authToken}");
                request.AddHeader("accept", "application/json");
                var resp = await client.ExecuteAsync(request);
                var token = JObject.Parse(resp.Content)["request_token"].ToString();
                Process.Start(new ProcessStartInfo($"https://www.themoviedb.org/authenticate/{token}"){UseShellExecute=true});
                if(MessageBox.Show("After authorizing click OK","Authorize",MessageBoxButtons.OKCancel)==DialogResult.OK)
                {
                    client  = new RestClient("https://api.themoviedb.org/3/authentication/session/new");
                    request = new RestRequest();
                    request.AddHeader("Authorization", $"Bearer {authToken}");
                    request.AddHeader("accept", "application/json");
                    request.AddJsonBody(new { request_token = token });
                    resp = await client.ExecuteAsync(request);
                    var sessionId = JObject.Parse(resp.Content)["session_id"].ToString();
                    var (added,msg) = await UserAppAccount.AddThirdPartyId(
                        UserAppAccount.TMDBPlatformID,
                        sessionId
                    );
                    if(added) await LoadTmdbAsync(); else MessageBox.Show($"Link failed: {msg}");
                }
            }
            catch(Exception ex){MessageBox.Show($"Error: {ex.Message}");}
        }
    }
}