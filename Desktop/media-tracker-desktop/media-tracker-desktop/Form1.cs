using media_tracker_desktop.Models;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.SupabaseTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Supabase;
using System.Configuration;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Testing
        private async void btnDBConnectionTest_Click(object sender, EventArgs e)
        {
            // make sure that, in the App.config file, the supabase api base url and key are entered.

            // enter the user and password here for testing.
            // If the Supabase [Enable read access for all users] policy is enabled, 
            // signing in shouldn't be necessary.
            //string userEmailDB = "";
            //string userPasswordDB = "";

            Client connection = new SupabaseConnection().GetClient();

            // test connection ----------
            //TestSupabaseConnection(connection);

            // test viewing tables -----------
            // to test different table, go to method.
            TestSupabaseViewTable(connection);

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

        private async void TestSupabaseViewTable(Client connection)
        {
            // Change the object type of the .From method to a different table model for testing.
            var records = await connection.From<UserFavorite>().Get();

            string testDisplay = "";

            foreach (var record in records.Models)
            {
                testDisplay += $"{record.ToString}\n";
            }

            MessageBox.Show(testDisplay);
        }

        private async void btnTestSteam_Click(object sender, EventArgs e)
        {
            //string steamUrl = ConfigurationManager.AppSettings["SteamApiOwnedGamesUrl"];

            // it doesn't seem to work if I put this in App.config, hence why it's here, otherwise, ideally, the above commented out code would be used.
            string steamUrl = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=5553B2F6E49998D47EB298C086A05084&steamid=76561198012120830&include_appinfo=1&format=json";

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
                var steamUserGamesJsonString = steamJson.Root["response"]["games"].ToString();

                // pass the json string to deserialize each game into steam_model objects
                List<Steam_Model> steamUserGames = JsonConvert.DeserializeObject<List<Steam_Model>>(steamUserGamesJsonString);

                string message = "";

                foreach (Steam_Model game in steamUserGames)
                {
                    message += $"{game.Name} - {game.RTimeLastPlayed} - {game.AppID} \n";
                }

                MessageBox.Show(message);
            }
        }
    }
}
