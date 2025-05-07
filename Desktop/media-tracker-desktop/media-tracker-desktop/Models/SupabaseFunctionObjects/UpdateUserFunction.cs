using Supabase;
using media_tracker_desktop.Models.SupabaseTables;

namespace media_tracker_desktop.Models.SupabaseFunctionObjects
{
    public static class UpdateUserFunction
    {
        private const string UPDATE_USER_SP_NAME = "update_user";
        private static Client _connection = UserAppAccount.GetConnection;

        public static async Task<Dictionary<string, dynamic>> UpdateUser(string newUsername = "", string newFirstName = "", string newLastName = "", string newEmail = "", string newPassword = "")
        {
            // Dictionary to be returned.
            Dictionary<string, dynamic> updateResult = new Dictionary<string, dynamic>
            {
                { "status", "" },
                { "statusMessage", "" }
            };

            // These variables are used to determine if a field changes or not.
            // They are set to have the same user details.
            // But when user updates a detail, they are stored in these variables, overriding the previous detail.
            // Passwords will update regardless if it's the same. Can't and shouldn't retrieve passwords.
            string updateUsername = UserAppAccount.Username;
            string updateFirstName = UserAppAccount.FirstName;
            string updateLastName = UserAppAccount.LastName;
            string updateEmail = UserAppAccount.Email;
            string updatePassword = newPassword;

            // If username changed,
            if (newUsername != updateUsername)
            {
                List<User> existingUsers = await SupabaseConnection.GetTableRecord<User>(_connection);

                // If username already exists in the database, return error.
                if (existingUsers.Any(u => u.Username == newUsername))
                {
                    updateResult["status"] = "error";
                    updateResult["statusMessage"] = "Username is already taken.";

                    return updateResult;
                }

                // Otherwise, update username.
                updateUsername = newUsername;
            }

            // If first name change, set new first name.
            if (newFirstName != updateFirstName)
            {
                updateFirstName = newFirstName;
            }

            // If last name change, set new last name.
            if (newLastName != updateLastName)
            {
                updateLastName = newLastName;
            }

            // If email change, set new email.
            if (newEmail != updateEmail)
            {
                updateEmail = newEmail;
            }

            try
            {
                // Parameter for SP with validations.
                UpdateUserParam updateUser = new UpdateUserParam(
                    updateUsername,
                    updateFirstName,
                    updateLastName,
                    updateEmail,
                    updatePassword
                );

                // Execute the stored procedure and wait for a response.
                var response = await _connection.Rpc(UPDATE_USER_SP_NAME, updateUser);

                // If success,
                if (response.Content.Contains("User has been updated"))
                {
                    updateResult["status"] = "success";
                    updateResult["statusMessage"] = "Successfully updated user.";

                    // Update session variables.
                    User updateUserVariables = new User();
                    updateUserVariables.Username = updateUsername;
                    updateUserVariables.FirstName = updateFirstName;
                    updateUserVariables.LastName = updateLastName;
                    updateUserVariables.Email = updateEmail;
                    updateUserVariables.Password = updatePassword;
                    UserAppAccount.UpdateUserSessionVariables(updateUserVariables);


                    return updateResult;
                }
                else
                {
                    updateResult["status"] = "error";
                    updateResult["statusMessage"] = response.Content ?? "An error occurred.";

                    return updateResult;
                }
            }
            catch (Exception error)
            {
                updateResult["status"] = "error";
                updateResult["statusMessage"] = error.Message;

                return updateResult;
            }
        }



    }
}
