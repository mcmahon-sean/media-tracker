using Supabase;
using Supabase.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public class SupabaseConnection
    {
        private Supabase.Client client;
        private dynamic session;

        // Store a user's email and password.
        // Used to allow CRUD operations on the DB.
        // In Supabase, it may be found under the Authentication tab, then Users.
        // May change.
        private string userEmailDB = "serviceaccount@gmail.com";
        private string userPasswordDB = "testaccount";

        public SupabaseConnection(string userEmailDB, string userPasswordDB)
        {
            _ = InitializeDB();
        }

        // Getter: Retrieve the database connection.
        // The object returned allows one to perform CRUD operations on the database.
        public Supabase.Client GetClient()
        {
            return client;
        }

        //public static Supabase.Functions.Client ExecuteFunction(string functionName)
        //{
            
        //}

        // Getter: Retrieve the session.
        // The object returned allows one to view the session's expiration, etc.
        public dynamic GetSession()
        {
            return session;
        }

        // Method: Initializes the DB
        // Returns "success" or an error message.
        private async Task<string> InitializeDB()
        {
            try
            {
                // Retrieve the base url and api key of the supabase DB.
                string supabaseBaseUrl = ConfigurationManager.AppSettings["SupabaseApiBaseUrl"]!;
                string supabaseApiKey = ConfigurationManager.AppSettings["SupabaseApiKey"]!;

                SupabaseOptions supabaseOptions = new SupabaseOptions
                {
                    // Auto connect to database, useful when different platforms can interact with the database performing CRUD operations.
                    AutoConnectRealtime = true,
                    // Auto refresh the session.
                    AutoRefreshToken = true
                };

                // Create connection.
                client = new Supabase.Client(supabaseBaseUrl, supabaseApiKey, supabaseOptions);

                // Connect to DB.
                await client.InitializeAsync();

                // Currently, auto sign's in a user.
                string signInMessage = await SignInUsingEmail();

                // Check if sign in is successful.
                if (signInMessage == "success")
                {
                    return "success";
                }
                else
                {
                    return "Failed to sign in.";
                }
            }
            // Return error message if any error occurs.
            catch (Exception error)
            {
                return error.Message;
            }
        }

        // Method: Signs in to DB using email.
        public async Task<string> SignInUsingEmail()
        {
            try
            {
                // If DB is not initialized,
                if (client == null)
                {
                    throw new Exception("Database has not been initialized.");
                }
                // Else,
                else
                {
                    // Sign in to DB.
                    session = await client.Auth.SignIn(userEmailDB, userPasswordDB);

                    // Failed to sign in if there is no session returned.
                    // Otherwise, successfully signed in.
                    return session == null ? "Failed to sign in." : "success";
                }
            }
            // Return error message if any error occurs.
            catch (Exception error)
            {
                return error.Message;
            }
        }
    }
}
