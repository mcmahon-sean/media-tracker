using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.SupabaseTables;
using Microsoft.IdentityModel.Tokens;
using Supabase;

namespace media_tracker_desktop.Models.SupabaseFunctionObjects
{
    public class Delete3rdPartyIDFunction
    {
        private const string DELETE_THIRD_PARTY_ID_SP_NAME = "delete_3rd_party";
        private static Client _connection = UserAppAccount.GetConnection;

        public static async Task<Dictionary<string, dynamic>> UnlinkApiAccount(int platformID)
        {
            Dictionary<string, dynamic> unlinkResult = new Dictionary<string, dynamic>
            {
                ["status"] = "",
                ["statusMessage"] = ""
            };

            string userPlatID = UserAppAccount.GetUserPlatformID(platformID);

            if (userPlatID.Contains("Invalid"))
            {
                unlinkResult["status"] = "error";
                unlinkResult["statusMessage"] = "Invalid platform ID.";

                return unlinkResult;
            }

            var unlinkParam = new
            {
                username_input = UserAppAccount.Username,
                platform_id_input = platformID,
                user_plat_id_input = userPlatID
            };

            // Execute the stored procedure and wait for a response.
            var response = await _connection.Rpc(DELETE_THIRD_PARTY_ID_SP_NAME, unlinkParam);

            if (response.Content.Contains("deleted"))
            {
                string platformName = UserAppAccount.GetPlatformName(platformID);

                unlinkResult["status"] = "success";
                unlinkResult["statusMessage"] = $"Successfully unlinked {platformName} account.";

                UserAppAccount.LogOutPlatform(platformID);

                return unlinkResult;
            }
            else
            {
                unlinkResult["status"] = "error";
                unlinkResult["statusMessage"] = response.Content;

                return unlinkResult;
            }
        }
    }
}
