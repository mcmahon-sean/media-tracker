using media_tracker_desktop.Models;
using Supabase;
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

            // Username is the table model. Can change the model to test a table.
            var records = await connection.From<Username>().Get();

            string testDisplay = "";

            foreach (var record in records.Models)
            {
                testDisplay += $"{record.Name} - {record.Age}\n";
            }

            MessageBox.Show(testDisplay);
        }
    }
}
