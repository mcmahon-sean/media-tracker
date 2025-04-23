using media_tracker_desktop.Models;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.LastFM;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Supabase;
using System.Configuration;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace media_tracker_desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Client connection;

        private string lastFMUrl;
        private string lastFMApiKey;

        // Testing
        private async void btnDBConnectionTest_Click(object sender, EventArgs e)
        {
            // make sure that, in the App.config file, the supabase api base url and key are entered.

            // enter the user and password here for testing.
            // If the Supabase [Enable read access for all users] policy is enabled, 
            // signing in shouldn't be necessary.
            //string userEmailDB = "";
            //string userPasswordDB = "";

            string? initializationResult = SupabaseConnection.InitializeDB().ToString();

            connection = SupabaseConnection.GetClient();

            // test connection ----------
            TestSupabaseConnection(connection);

            // test viewing tables -----------
            // to test different table, go to method.
            //TestSupabaseViewUserAccountTable(connection);
            var records = await SupabaseConnection.GetTableRecord<UserAccount>(connection);

            string displayString = "";

            foreach (var record in records)
            {
                displayString += record.UserPlatID + " " + record.Username + " " + record.PlatformID + "\n\n";
            }

            MessageBox.Show(displayString);

            // Initializing connection.
            UserAppAccount.ConnectToDB(connection);


            // initializing last fm stuff
            lastFMUrl = ConfigurationManager.AppSettings["LastFMApiBaseUrl"];
            lastFMApiKey = ConfigurationManager.AppSettings["LastFMApiKey"];

            LastFMApi.Initialize(lastFMUrl, lastFMApiKey);
            
        }

        // Testing user create. Normally should be done in a separate form.
        private async void btnTestUserCreate_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                MessageBox.Show("Not connected to the DB.");
                return;
            }



            // variable was used to control a while loop, which loops again if the user didn't enter valid fields to create a new user.
            bool validUser = false;


            // Default Username = "testDesktopUser"
            // Default Password = "testDesktopPassword"
            string username = txtUsername.Text;
            string firstName = "testDesktopFN";
            string lastName = "testDesktopLN";
            string email = "testDesktop@email.com";
            string password = txtPassword.Text;

            UserRegistrationParam newUser = null;

            // try catch block is implemented since the SupabaseUserRegistration object throws exception as a means of validation.
            try
            {
                newUser = new UserRegistrationParam(username, firstName, lastName, email, password);

                // If the user object is not null,
                if (newUser != null)
                {
                    // Create the user.
                    (bool userCreated, string message) result = await UserAppAccount.CreateUser(newUser);

                    // If the creation is successful,
                    if (result.userCreated)
                    {
                        MessageBox.Show($"User: {newUser.Username} created.");

                        validUser = true;
                    }
                    else
                    {
                        MessageBox.Show(result.message);

                        validUser = false;
                    }
                }
                else
                {
                    validUser = false;
                }
            }
            catch (Exception error)
            {
                validUser = false;
            }
        }

        //(Jacob A, 4/12) Opens the signup page, logging in as the user on successful account creation.
        private async void btnUserSignupForm_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                MessageBox.Show("Not connected to the DB.");
                return;
            }
        }
        private async void btnTestUserLogin_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                MessageBox.Show("Not connected to the DB.");
                return;
            }

            bool validUser = false;

            // Default Username = "testDesktopUser"
            // Default Password = "testDesktopPassword"

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            UserLoginParam user = null;

            // try catch block is implemented since the SupabaseUserLogin object throws exception as a means of validation.
            try
            {
                user = new UserLoginParam(username, password);

                // If the user object is not null,
                if (user != null)
                {
                    // Create the user.
                    (bool userAuthenticated, string message) result = await UserAppAccount.AuthenticateUser(user);

                    // If the user is authenticated,
                    if (result.userAuthenticated)
                    {
                        MessageBox.Show($"{user.Username} logged in.");
                        if (UserAppAccount.UserSteamID != null)
                        {
                            txtLinkingBox.Text = UserAppAccount.UserSteamID;
                        }

                        validUser = true;

                        // connect api's
                        LastFMApi.User = UserAppAccount.UserLastFmID;
                    }
                    else
                    {
                        MessageBox.Show(result.message);

                        validUser = false;
                    }
                }
                else
                {
                    validUser = false;
                }
            }
            catch (Exception error)
            {
                validUser = false;
            }
        }

        private async void TestSupabaseConnection(Client connection)
        {
            string testDisplay = "";

            var param = new Dictionary<string, string>
            {
                {"app", "Desktop" }
            };

            var data = await connection.Rpc("testconnectionwitharguments", param);

            testDisplay = data.Content.ToString();

            MessageBox.Show(testDisplay);
        }

        private async void btnTestSteam_Click(object sender, EventArgs e)
        {
            //string steamUrl = ConfigurationManager.AppSettings["SteamApiOwnedGamesUrl"];

            // it doesn't seem to work if I put this in App.config, hence why it's here, otherwise, ideally, the above commented out code would be used.
            string steamBaseUrl = ConfigurationManager.AppSettings["SteamApiBaseUrl"];
            string steamApiKey = ConfigurationManager.AppSettings["steamApiKey"];
            string steamFormat = ConfigurationManager.AppSettings["SteamAPIFormat"];
            string steamUserID = UserAppAccount.UserSteamID;
            string steamIncludes = "&include_appinfo=1&include_played_free_games=1&format=json";
            string steamUrl = $"{steamBaseUrl}key={steamApiKey}&steamid={steamUserID}&include_appinfo=1&format={steamFormat}";

            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(steamUrl);

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            // if successful,
            if (response.IsSuccessful)
            {
                // convert the content to a json object
                var steamJson = JObject.Parse(response.Content);

                // retrieve the games property of the json object and convert it back to a json string
                var steamUserGamesJson = steamJson.Root["response"]["games"];

                if (steamUserGamesJson != null)
                {
                    // pass the json string to deserialize each game into steam_model objects
                    List<Steam_Model> steamUserGames = JsonConvert.DeserializeObject<List<Steam_Model>>(steamUserGamesJson.ToString());

                    string message = "";

                    foreach (Steam_Model game in steamUserGames)
                    {
                        message += $"{game.Name} - {game.RTimeLastPlayed} - {game.AppID} \n";
                    }

                    MessageBox.Show(message);
                }
                else
                {
                    MessageBox.Show("steamUserGamesJson is null.");
                }

            }
            
        }

        private async void btnTestLastFMArtist_Click(object sender, EventArgs e)
        {
            (bool isSuccess, List<LastFM_Artist>? artists) result = await LastFMApi.GetUserTopArtists();

            if (result.isSuccess && result.artists != null)
            {
                string message = "";
                
                foreach (LastFM_Artist artist in result.artists)
                {
                    message += $"{artist.Name} - {artist.ImageUrl} \n";
                }

                MessageBox.Show(message);
            }
        }

        private async void btnTestLastFMTrack_Click(object sender, EventArgs e)
        {
            (bool isSuccess, List<LastFM_Track>? tracks) result = await LastFMApi.GetUserRecentTracks();

            if (result.isSuccess && result.tracks != null)
            {
                string message = "";

                foreach (LastFM_Track track in result.tracks)
                {
                    message += $"{track.ArtistName} - {track.ImageUrl} - {track.Timestamp} \n";
                }

                MessageBox.Show(message);
            }
        }

        private async void btnTestLastFMUser_Click(object sender, EventArgs e)
        {
            (bool isSuccess, LastFM_User? user) result = await LastFMApi.GetUserInfo();

            if (result.isSuccess && result.user != null)
            {

                MessageBox.Show($"{result.user.Name} - {result.user.IsSubscriber} - {result.user.RegisteredAt}");
            }
        }

        private async void btnTestTMDBAccount_Click(object sender, EventArgs e)
        {
            string tmdbBaseUrl = ConfigurationManager.AppSettings["TMDBApiBaseUrl"];
            string tmdbAuthToken = ConfigurationManager.AppSettings["TMDBNathanAuthToken"];

            string tmdbUrl = $"{tmdbBaseUrl}account_id?session_id={UserAppAccount.UserTmdbSessionID}";

            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(tmdbUrl);

            request.AddHeader("Authorization", $"Bearer {tmdbAuthToken}");

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // Deserialize
                TMDB_Account accountInfo = JsonConvert.DeserializeObject<TMDB_Account>(response.Content);


                MessageBox.Show($"{accountInfo.ID} - {accountInfo.Username}");
            }
            else
            {
                MessageBox.Show(response.Content);
            }
        }

        private async void btnTestTMDBMovie_Click(object sender, EventArgs e)
        {
            string tmdbBaseUrl = ConfigurationManager.AppSettings["TMDBApiBaseUrl"];
            string tmdbAuthToken = ConfigurationManager.AppSettings["TMDBNathanAuthToken"];
            string tmdbAccountId = UserAppAccount.UserTmdbAccountID;
            string tmdbSessionId = UserAppAccount.UserTmdbSessionID;

            string tmdbUrl = $"{tmdbBaseUrl}{tmdbAccountId}/rated/movies";

            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(tmdbUrl);

            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);
            request.AddParameter("sort_by", "created_at.desc");
            request.AddParameter("session_id", tmdbSessionId);

            request.AddHeader("Authorization", $"Bearer {tmdbAuthToken}");

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // Convert content to json object.
                var movieResultJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var movieJson = movieResultJson.Root["results"];

                // Deserialize
                List<TMDB_Movie> movies = JsonConvert.DeserializeObject<List<TMDB_Movie>>(movieJson.ToString());

                string message = "";

                foreach (TMDB_Movie movie in movies)
                {
                    message += $"{movie.ID} - {movie.Title} - {movie.Overview} \n\n";
                }

                MessageBox.Show(message);
            }
        }

        private async void btnTestTMDBTVShow_Click(object sender, EventArgs e)
        {
            string tmdbBaseUrl = ConfigurationManager.AppSettings["TMDBApiBaseUrl"];
            string tmdbAuthToken = ConfigurationManager.AppSettings["TMDBNathanAuthToken"];
            string tmdbAccountId = UserAppAccount.UserTmdbAccountID;
            string tmdbSessionId = UserAppAccount.UserTmdbSessionID;

            string tmdbUrl = $"{tmdbBaseUrl}{tmdbAccountId}/favorite/tv";

            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(tmdbUrl);

            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);
            request.AddParameter("sort_by", "created_at.asc");
            request.AddParameter("session_id", tmdbSessionId);

            request.AddHeader("Authorization", $"Bearer {tmdbAuthToken}");

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // Convert content to json object.
                var tvShowResultJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var tvShowJson = tvShowResultJson.Root["results"];

                // Deserialize
                List<TMDB_TV_Show> tvShows = JsonConvert.DeserializeObject<List<TMDB_TV_Show>>(tvShowJson.ToString());

                string message = "";

                foreach (TMDB_TV_Show tvShow in tvShows)
                {
                    message += $"{tvShow.ID} - {tvShow.Name} - {tvShow.Overview} \n\n";
                }

                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show(response.Content);
            }
        }

        // Test Method to make sure UserAppAccount is returning a username after logging in
        private void btnGetUser_Click(object sender, EventArgs e)
        {
            if (UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show(UserAppAccount.Username);
            }
            else
            {
                MessageBox.Show("User Not Logged in");
            }

        }
        private async void AccountLinking(int PlatformID, string UserPlatformID)
        {
            try
            {
                if (!UserAppAccount.UserLoggedIn)
                {
                    MessageBox.Show("User Not Logged in");
                }
                if (connection == null)
                {
                    MessageBox.Show("Not connected to the DB.");
                }
                var UserPlatformId = UserPlatformID;
                var PlatformId = PlatformID;

                (bool linkAdded, string response) result = await UserAppAccount.AddThirdPartyId(PlatformId, UserPlatformId);
                if (result.linkAdded)
                {
                    MessageBox.Show($"Success!: {result.response}");
                }
                else
                {
                    MessageBox.Show($"Failed: {result.response}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void btnLinkSteam_Click(object sender, EventArgs e)
        { 
            string steamID = txtLinkingBox.Text;
            AccountLinking(UserAppAccount.SteamPlatformID, steamID);


            string steamBaseUrl = ConfigurationManager.AppSettings["SteamApiBaseUrl"];
            string steamApiKey = ConfigurationManager.AppSettings["steamApiKey"];
            string steamFormat = ConfigurationManager.AppSettings["SteamAPIFormat"];
            string steamUserID = UserAppAccount.UserSteamID;
            string steamIncludes = "&include_appinfo=1&include_played_free_games=1&format=json";
            string steamUrl = $"{steamBaseUrl}?key={steamApiKey}&steamid={steamUserID}&include_appinfo=1&format={steamFormat}";


            var client = new RestClient();
            var request = new RestRequest(steamUrl);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var steamJson = JObject.Parse(response.Content);

                var steamUserGamesJson = steamJson.Root["response"]["games"];

                if (steamUserGamesJson != null)
                {
                    List<Steam_Model> steamUserGames = JsonConvert.DeserializeObject<List<Steam_Model>>(steamUserGamesJson.ToString());
                }
                else
                {
                    MessageBox.Show("SteamId not found!");
                }
            }
        }

        private async void btnLinkLastFm_Click(object sender, EventArgs e)
        {
            string lastFMUrl = ConfigurationManager.AppSettings["LastFMApiBaseUrl"];
            string lastFMApiKey = ConfigurationManager.AppSettings["LastFMApiKey"];
            string lastFMUsername = ConfigurationManager.AppSettings["LastFMApiUsername"];

            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(lastFMUrl);

            request.AddParameter("method", "user.getTopArtists");
            request.AddParameter("user", lastFMUsername);
            request.AddParameter("api_key", lastFMApiKey);
            request.AddParameter("limit", 5);
            request.AddParameter("format", "json");

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                AccountLinking(UserAppAccount.LastFMPlatformID, lastFMUsername);
            }
            else
            {
                MessageBox.Show("Username not found!");
            }
            //string lastFMUsername = ConfigurationManager.AppSettings["LastFMApiUsername"]; 

        }

        private async void btnLinkTmdb_Click(object sender, EventArgs e)
        {
            var authHeader = $"Bearer {ConfigurationManager.AppSettings["TMDBNathanAuthToken"]}";
            var options = new RestClientOptions("https://api.themoviedb.org/3/authentication/token/new");
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", authHeader);

            var response = await client.GetAsync(request);

            if (response.IsSuccessful)
            {
                var requestToken = JObject.Parse(response.Content)["request_token"].ToString();

                Console.WriteLine("{0}", response.Content);
                MessageBox.Show(requestToken);

                var requestTokenUrl = $"https://www.themoviedb.org/authenticate/{requestToken}";
                //System.Diagnostics.Process is used to open the authentication in a new window
                System.Diagnostics.Process.Start(new ProcessStartInfo(requestTokenUrl)
                {
                    UseShellExecute = true
                });

                DialogResult result = MessageBox.Show("One you have authorised the app press continue.", "Continue?", MessageBoxButtons.OK);
                //Response if a guarenteed failure if user doesnt hit approve before hitting ok
                if (result == DialogResult.OK)
                {
                    try
                    {
                        string jsonBodyToken = requestToken;

                        options = new RestClientOptions("https://api.themoviedb.org/3/authentication/session/new");
                        client = new RestClient(options);
                        request = new RestRequest("");
                        request.AddHeader("accept", "application/json");
                        request.AddHeader("Authorization", authHeader);
                        request.AddJsonBody(new { request_token = requestToken });

                        response = await client.PostAsync(request);
                        var success = JObject.Parse(response.Content)["success"].ToString();

                        if (success == "True")
                        {
                            var sessionID = JObject.Parse(response.Content)["session_id"].ToString();
                            MessageBox.Show(sessionID);
                            AccountLinking(UserAppAccount.TMDBPlatformID, sessionID);

                        }
                        else
                        {
                            MessageBox.Show(response.Content);
                        }
                    }
                    catch (Exception ex)
                    {
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
        }

        private void btnCheckTMDB_Click(object sender, EventArgs e)
        {
            if (UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show($"TMDB Account Linked: {UserAppAccount.UserTmdbAccountID} | {UserAppAccount.UserTmdbSessionID}");
            }
            else
            {
                MessageBox.Show("User Not Logged in");
            }
        }

        private void btnCheckLastFM_Click(object sender, EventArgs e)
        {
            if (UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show($"LastFM Account Linked: {UserAppAccount.UserLastFmID}");
            }
            else
            {
                MessageBox.Show("User Not Logged in");
            }
        }

        private void btnCheckSteam_Click(object sender, EventArgs e)
        {
            if (UserAppAccount.UserLoggedIn)
            {
                MessageBox.Show($"Steam Account Linked: {UserAppAccount.UserSteamID}");
            }
            else
            {
                MessageBox.Show("User Not Logged in");
            }

        }
    }
}
