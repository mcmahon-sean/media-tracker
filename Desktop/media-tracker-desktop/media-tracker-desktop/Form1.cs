using media_tracker_desktop.Models;
using Supabase;
using Supabase.Functions;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json.Nodes;
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
            string userEmailDB = "serviceaccount@gmail.com";
            string userPasswordDB = "8f65dee5-8557-4da7-9d27-3bbba7a8bf4a";

            Supabase.Client connection = new SupabaseConnection(userEmailDB, userPasswordDB).GetClient();

            // Username is the table model. Can change the model to test a table.
            //var records = await connection.From<Username>().Get();

            //var records = await connection.From<MediaTypes>().Get();

            string testDisplay = "";

            //var data = await connection.Rpc("testconnection", null);

            //testDisplay = data.Content.ToString();


            var param = new Dictionary<string, string>
            {
                {"app", "Desktop" }
            };

            var data = await connection.Rpc("testconnectionwitharguments", param);
            testDisplay = data.Content.ToString();

            MessageBox.Show(testDisplay);
        }
    }
}
