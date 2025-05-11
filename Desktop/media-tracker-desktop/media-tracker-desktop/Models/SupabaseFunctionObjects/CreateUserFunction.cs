using Supabase;


namespace media_tracker_desktop.Models.SupabaseFunctionObjects
{
    public static class CreateUserFunction
    {
        private const string CREATE_USER_SP_NAME = "CreateUser";
        private static Client _connection = UserAppAccount.GetConnection;

        public static async Task<(bool, string)> CreateUser(UserRegistrationParam newUser)
        {

            // If the user object is null,
            if (newUser == null)
            {
                return (false, "New user object is null.");
            }

            try
            {
                // Execute the stored procedure and wait for a response.
                var response = await _connection.Rpc(CREATE_USER_SP_NAME, newUser);

                // Convert the returned value from the stored procedure to a boolean.
                bool returnedBool = Boolean.TryParse(response.Content, out bool userCreated);

                // If the conversion failed,
                if (!returnedBool)
                {
                    // Return result to indicate that the DB returned a non-boolean.
                    return (returnedBool, "Database did not return a boolean value.");
                }

                // Message depends on whether or not the stored procedure successfully created the user.
                string message;

                if (userCreated)
                {
                    message = "Successfully created user.";
                }
                else
                {
                    message = "Failed to create user.";
                }

                // Return the result.
                return (userCreated, message);
            }
            catch (Exception error)
            {
                return (false, error.Message);
            }
        }
    }
}
