using media_tracker_desktop.Models;
using media_tracker_desktop.Models.LastFM;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Supabase;
using System;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace media_tracker_desktop
{
    public partial class SigninForm : Form
    {
        // Field to store the Supabase connection.
        private Client connection;

        // Constructor that accepts a Supabase connection
        public SigninForm(Client client)
        {
            // Assign the passed connection to our field
            //connection = client;

            UserAppAccount.ConnectToDB(client);

            InitializeComponent();
        }

        // Event handler for the login button click
        private async void btnTestUserLogin_Click(object sender, EventArgs e)
        {
            // Ensure we have a connection to Supabase before proceeding
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
    }
}
