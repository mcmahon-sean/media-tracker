using Supabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace media_tracker_desktop
{
    public static class SupabaseConnection
    {
        private static Client _client;
        private static dynamic _session;

        // Store a user's email and password.
        // Used to allow CRUD operations on the DB.
        // In Supabase, it may be found under the Authentication tab, then Users.
        // May change.
        private static string userEmailDB = "serviceaccount@gmail.com";
        private static string userPasswordDB = "testaccount";

        // Getter: Retrieve the database connection.
        // The object returned allows one to perform CRUD operations on the database.
        public static Client GetClient()
        {
            return _client;
        }

        // Getter: Retrieve the session.
        // The object returned allows one to view the session's expiration, etc.
        public static dynamic GetSession()
        {
            return _session;
        }

        // Method: Initializes the DB
        // Returns "success" or an error message.
        public static async Task<string> InitializeDB()
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
                _client = new Client(supabaseBaseUrl, supabaseApiKey, supabaseOptions);

                // Connect to DB.
                await _client.InitializeAsync();

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
        public static async Task<string> SignInUsingEmail()
        {
            try
            {
                // If DB is not initialized,
                if (_client == null)
                {
                    throw new Exception("Database has not been initialized.");
                }
                // Else,
                else
                {
                    // Sign in to DB.
                    _session = await _client.Auth.SignIn(userEmailDB, userPasswordDB);

                    // Failed to sign in if there is no session returned.
                    // Otherwise, successfully signed in.
                    return _session == null ? "Failed to sign in." : "success";
                }
            }
            // Return error message if any error occurs.
            catch (Exception error)
            {
                return error.Message;
            }
        }

        // Method: View a table in the database.
        public static async Task<List<T>> GetTableRecord<T>(Client connection) where T : Supabase.Postgrest.Models.BaseModel, new()
        {
            var result = await connection.From<T>().Get();

            return result.Models;
        }
    }
}
