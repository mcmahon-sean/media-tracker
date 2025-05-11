using media_tracker_desktop.Models.SupabaseTables;
using Microsoft.IdentityModel.Tokens;
using Supabase;

namespace media_tracker_desktop.Models.SupabaseFunctionObjects
{
    public class Add3rdPartyIDFunction
    {
        private const string ADD_THIRD_PARTY_ID = "add_3rd_party_id";
        private static Client _connection = UserAppAccount.GetConnection;
        public static async Task<(bool, string)> UpdateUserPlatformID(int platformID, string newUserPlatformID)
        {
            // Retrieve the platform name, for display purposes.
            string platformName = UserAppAccount.GetPlatformName(platformID);

            // If there is no new user platform ID,
            if (string.IsNullOrEmpty(newUserPlatformID))
            {
                // Return false.
                return (false, $"New User {platformName} ID is null.");
            }
            // Else,
            else
            {
                // Update the user's platform ID.
                // The SP updates/adds the user's platform ID.
                (bool updateSuccess, string message) = await AddThirdPartyId(platformID, newUserPlatformID);

                if (updateSuccess)
                {
                    return (true, $"User {platformName} ID updated.");
                }
                else
                {
                    return (false, $"Unable to update user {platformName} ID");
                }
            }
        }
        public static async Task<(bool, string)> AddThirdPartyId(int? PlatformId, string UserPlatformId)
        {
            string userName = UserAppAccount.Username;
            var parameters = new 
            {
                username_input = userName,
                platform_id_input = PlatformId,
                user_plat_id_input = UserPlatformId
            };

            if (!UserAppAccount.UserLoggedIn)
            {
                return (false, "User is not logged in.");
            }

            if (!PlatformId.HasValue)
            {
                return (false, "Platform ID is null.");
            }

            if (string.IsNullOrEmpty(UserPlatformId))
            {
                return (false, "User Platform ID is null.");
            }

            if (string.IsNullOrEmpty(userName))
            {
                return (false, "Username is null.");
            }

            if (_connection == null)
            {
                return (false, "Connection is null.");
            }

            try
            {
                var response = await _connection.Rpc(ADD_THIRD_PARTY_ID, parameters);

                string returnedString = response.Content != null ? response.Content.ToString() : string.Empty;

                if (returnedString.IsNullOrEmpty())
                {
                    return (false, "Database did not return a string value.");
                }

                // Update the corresponding user api id session variables and for each api model.
                List<UserAccount> listAccount = new List<UserAccount>();
                User blankUser = new User();
                UserAccount acct = new();
                acct.Username = userName;
                acct.PlatformID = (int)PlatformId;
                acct.UserPlatID = UserPlatformId;

                listAccount.Add(acct);

                await UserAppAccount.UpdateUserSessionVariables(blankUser, listAccount);

                return (true, returnedString);
            }
            catch (Exception error)
            {
                return (false, error.Message);

            }
        }
    }
}
