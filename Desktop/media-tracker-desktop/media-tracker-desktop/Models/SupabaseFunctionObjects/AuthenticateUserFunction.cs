using media_tracker_desktop.Models.SupabaseTables;
using Supabase;


namespace media_tracker_desktop.Models.SupabaseFunctionObjects
{
    public class AuthenticateUserFunction
    {
        private const string AUTHENTICATE_USER_SP_NAME = "AuthenticateUser";
        private static Client _connection = UserAppAccount.GetConnection;


        public static async Task<(bool, string)> AuthenticateUser(UserLoginParam user)
        {
            // Since we are attempting to log in a new user, current user is logged out.
            UserAppAccount.LogOut();

            // If the user object is null,
            if (user == null)
            {
                return (false, "User object is null.");
            }

            try
            {
                // Execute the stored procedure and wait for a response.
                var response = await _connection.Rpc(AUTHENTICATE_USER_SP_NAME, user);

                // Convert the returned value from the stored procedure to a boolean.
                bool returnedBool = Boolean.TryParse(response.Content, out bool userAuthenticated);

                // If the conversion failed,
                if (!returnedBool)
                {
                    // Return result to indicate that the DB returned a non-boolean.
                    return (returnedBool, "Database did not return a boolean value.");
                }

                // Message depends on whether or not the stored procedure successfully created the user.
                string message;

                if (userAuthenticated)
                {
                    // Retrieve user details.
                    // Filtered by username.
                    var userQueryResult = await _connection.From<User>().Filter(u => u.Username, Supabase.Postgrest.Constants.Operator.Equals, user.Username).Get();

                    // Retrieve user account(s).
                    // Filtered by username.
                    var userAccountQueryResult = await _connection.From<UserAccount>().Filter(u => u.Username, Supabase.Postgrest.Constants.Operator.Equals, user.Username).Get();

                    // User record shouldn't be null since the user is logged in at this stage. Hence, (!).
                    await UserAppAccount.UpdateUserSessionVariables(userQueryResult.Model!, userAccountQueryResult.Models);

                    message = "Successfully logged in.";
                }
                else
                {
                    message = "Failed to log in.";
                }

                // Return the result.
                return (userAuthenticated, message);
            }
            catch (Exception error)
            {
                return (false, error.Message);
            }
        }

        
    }
}
