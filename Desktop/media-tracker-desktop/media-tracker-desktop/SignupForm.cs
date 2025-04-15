using media_tracker_desktop.Models;
using media_tracker_desktop.Models.LastFM;
using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Supabase;
using System.Configuration;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
        }

        private Client connection;
        public Client GetConnection() { return connection; }

        //Button code copied from Form1.cs
        private async void btnTestUserCreate_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                MessageBox.Show("Not connected to the DB.");
                return;
            }

            UserAppAccount.ConnectToDB(connection);

            // variable was used to control a while loop, which loops again if the user didn't enter valid fields to create a new user.
            bool validUser = false;


            // Default Username = "testDesktopUser"
            // Default Password = "testDesktopPassword"
            string username = txtUsername.Text;
            string firstName = txtFN.Text;
            string lastName = txtLN.Text;
            string email = txtEmail.Text;
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

    }
}
