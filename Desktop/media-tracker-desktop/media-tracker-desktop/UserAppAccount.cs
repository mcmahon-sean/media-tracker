using media_tracker_desktop.Models;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using Supabase;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace media_tracker_desktop
{
    public static class UserAppAccount
    {
        // Stored Procedure Names
        private const string CREATE_USER_SP_NAME = "CreateUser";
        private const string AUTHENTICATE_USER_SP_NAME = "AuthenticateUser";
        private const string ADD_THIRD_PARTY_ID = "add_3rd_party_id";
        private const string UPDATE_USER_SP_NAME = "update_user";
        private const string DELETE_USER_SP_NAME = "delete_user";

        private const int STEAM_PLATFORM_ID = 1;
        private const int LASTFM_PLATFORM_ID = 2;
        private const int TMDB_PLATFORM_ID = 3;

        private const string INITIAL_MEDIA_FAV_SP_NAME = "initial_media_fav";

        private static Client _connection;

        private static bool _userLoggedIn = false;

        private static string _username = string.Empty;
        private static string _firstName = string.Empty;
        private static string _lastName = string.Empty;
        private static string _email = string.Empty;

        private static string _userLastFmID = string.Empty;
        private static string _userSteamID = string.Empty;
        private static string _userTmdbSessionID = string.Empty;
        private static string _userTmdbAccountID = string.Empty;

        public enum MediaTypeID
        {
            Game = 1,
            TV_Show = 2,
            Film = 3,
            Song = 4,
            Album = 5,
            Artist = 6
        }

        // ----- Getter Methods -----
        // Method: Returns bool if a user is logged in.
        // Only getters.
        // Reason:
        // These are values from the database.
        // If these values were to be updated outside of this class, we won't know if it is also updated in the database.
        // These values should only contain values that are in the database.
        public static bool UserLoggedIn
        {
            get { return _userLoggedIn; }
        }

        public static string Username
        {
            get { return _username; }
        }

        public static string FirstName
        {
            get { return _firstName; }
        }

        public static string LastName
        {
            get { return _lastName; }
        }

        public static string Email
        {
            get { return _email; }
        }

        public static int SteamPlatformID
        {
            get { return STEAM_PLATFORM_ID; }
        }
        public static int LastFMPlatformID
        {
            get { return LASTFM_PLATFORM_ID; }
        }
        public static int TMDBPlatformID
        {
            get { return TMDB_PLATFORM_ID;}
        }

        public static string UserSteamID
        {
            get { return _userSteamID; }
        }

        public static string UserLastFmID
        {
            get { return _userLastFmID; }
        }

        public static string UserTmdbAccountID
        {
            get { return _userTmdbAccountID; }
        }

        public static string UserTmdbSessionID
        {
            get { return _userTmdbSessionID; }
        }
        // ----- Getter Methods END -----

        // Method: Connect to the DB using the passed connection.
        public static void ConnectToDB(Client dbConnection)
        {
            _connection = EnsureConnectionNotNull(dbConnection);
        }

        // Method: Ensures that connection is not null.
        private static Client EnsureConnectionNotNull(Client conn)
        {
            if (conn == null)
            {
                throw new ArgumentNullException($"Connection is null.");
            }

            return conn;
        }


        /// <summary>
        /// Method to create the user, which uses the CreateUser SP from the DB and passes a UserRegistrationParam as the parameter object.
        /// </summary>
        /// <param name="newUser">The object that represents the parameters for the CreateUser SP</param>
        /// <returns>
        /// Returns a (bool, string) tuple. Boolean indicates if the process of creating the user is successful. String is the message to help with debugging.
        /// </returns>
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

        /// <summary>
        /// Method to authenticate the user, which uses the AuthenticateUser SP from the DB and passes a UserLoginParam as the parameter object.
        /// </summary>
        /// <param name="user">The object that represents the parameters for the AuthenticateUser SP</param>
        /// <returns>Returns a (bool, string) tuple. Boolean indicates if the process of authenticating is successful. String is the message to help with debugging.</returns>
        public static async Task<(bool, string)> AuthenticateUser(UserLoginParam user)
        {
            // Since we are attempting to log in a new user, current user is logged out.
            LogOut();

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
                    await UpdateUserSessionVariables(userQueryResult.Model!, userAccountQueryResult.Models);

                    message = "Successfully logged in.";
                    _userLoggedIn = true;
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

        /// <summary>
        /// Updates the session variables.
        /// When a user is logged in, the session user api id variables are also updated, as well as the user's id in each api model.
        /// </summary>
        /// <param name="user">The user object of the logged in user.</param>
        /// <param name="userAccounts">The list of user account objects that is associated with the user.</param>
        private static async Task UpdateUserSessionVariables(User user, List<UserAccount> userAccounts)
        {
            _username = user.Username;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _email = user.Email;

            // Foreach platform account that the user has,
            foreach (UserAccount userAccount in userAccounts)
            {
                // Determine the platform and store that user's id for that platform.
                // Also, input those ID into the api models.
                switch (userAccount.PlatformID)
                {
                    case STEAM_PLATFORM_ID:
                        _userSteamID = userAccount.UserPlatID.ToString();
                        SteamApi.SteamID = _userSteamID;

                        break;
                    case LASTFM_PLATFORM_ID:
                        _userLastFmID = userAccount.UserPlatID.ToString();
                        LastFMApi.User = _userLastFmID;

                        break;
                    case TMDB_PLATFORM_ID:
                        _userTmdbSessionID = userAccount.UserPlatID.ToString();
                        TmdbApi.SessionID = _userTmdbSessionID;

                        var (accountIDFound, accountID) = await GetTmdbAccountID(userAccount.UserPlatID.ToString());

                        if (accountIDFound)
                        {
                           _userTmdbAccountID = accountID;
                           TmdbApi.AccountID = _userTmdbAccountID;
                        }
                        else
                        {
                            Console.WriteLine("Account Id Not Found");
                        }
                        
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Update user details.
        /// </summary>
        /// <param name="newUsername"></param>
        /// <param name="newFirstName"></param>
        /// <param name="newLastName"></param>
        /// <param name="newEmail"></param>
        /// <param name="newPassword"></param>
        /// <returns>Returns a dictionary<string, dynamic> where string has status { "error", "success" } and the second part is the statusMessage.</returns>
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
            string updateUsername = _username;
            string updateFirstName = _firstName;
            string updateLastName = _lastName;
            string updateEmail = _email;
            string updatePassword = newPassword;

            // If username changed,
            if (newUsername != _username)
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
            if (newFirstName != _firstName)
            {
                updateFirstName = newFirstName;
            }

            // If last name change, set new last name.
            if (newLastName != _lastName)
            {
                updateLastName = newLastName;
            }

            // If email change, set new email.
            if (newEmail != _email)
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
                    _username = newUsername;
                    _firstName = newFirstName;
                    _lastName = newLastName;
                    _email = newEmail;

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

        // Method: Delete user.
        public static async Task<Dictionary<string, dynamic>> DeleteUser()
        {
            Dictionary<string, dynamic> deleteResult = new Dictionary<string, dynamic>
            {
                ["status"] = "",
                ["statusMessage"] = ""
            };

            var deleteUserParam = new
            {
                username_input = _username
            };        

            // Execute the stored procedure and wait for a response.
            var response = await _connection.Rpc(DELETE_USER_SP_NAME, deleteUserParam);

            if (response.Content.Contains("User has been deleted"))
            {
                deleteResult["status"] = "success";
                deleteResult["statusMessage"] = "Successfully deleted account.";

                LogOut();

                return deleteResult;
            }
            else
            {
                deleteResult["status"] = "error";
                deleteResult["statusMessage"] = response.Content;

                return deleteResult;
            }
        }

        // Method: Resets the session variables.
        public static void LogOut()
        {
            _userLoggedIn = false;

            _username = string.Empty;
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;

            _userLastFmID = string.Empty;
            _userSteamID= string.Empty;
            _userTmdbSessionID = string.Empty;
            _userTmdbAccountID = string.Empty;

            LastFMApi.Logout();
            SteamApi.Logout();
            TmdbApi.Logout();
        }

        /// <summary>
        /// When adding an api id, the session variables for user's api ids are also updated.
        /// In addition, the user's id in each api models are also updated.
        /// </summary>
        /// <param name="PlatformId"></param>
        /// <param name="UserPlatformId">Steam: the steam ID. LastFM: the lastFM username. TMDB: the TMDB session ID.</param>
        /// <returns></returns>
        public static async Task<(bool, string)> AddThirdPartyId(int? PlatformId, string UserPlatformId)
        {
            var parameters = new{
                username_input = _username,
                platform_id_input = PlatformId,
                user_plat_id_input = UserPlatformId
            };

            if (!_userLoggedIn)
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

            if (string.IsNullOrEmpty(_username))
            {
                return (false, "Username is null.");
            }

            if (_connection == null)
            {
                return (false, "Connection is null.");
            }

            try{
                var response = await _connection.Rpc(ADD_THIRD_PARTY_ID, parameters);

                string returnedString = response.Content != null ? response.Content.ToString() : string.Empty;

                if (returnedString.IsNullOrEmpty()){
                    return (false, "Database did not return a string value.");
                }

                // Update the corresponding user api id session variables and for each api model.
                switch (PlatformId)
                {
                    case STEAM_PLATFORM_ID:
                        _userSteamID = UserPlatformId;
                        SteamApi.SteamID = _userSteamID;

                        break;
                    case LASTFM_PLATFORM_ID:
                        _userLastFmID = UserPlatformId;
                        LastFMApi.User = _userLastFmID;

                        break;
                    case TMDB_PLATFORM_ID:
                        _userTmdbSessionID = UserPlatformId;
                        TmdbApi.SessionID = _userTmdbSessionID;

                        (bool accountIDFound, string accountID) = await GetTmdbAccountID(_userTmdbSessionID);

                        if (accountIDFound)
                        {
                            _userTmdbAccountID = accountID;
                            TmdbApi.AccountID = _userTmdbAccountID;
                        }
                        else
                        {
                            Console.WriteLine("Account Id Not Found");
                        }

                        break;
                    default:
                        break;
                }

                return (true, returnedString);
            }
            catch (Exception error)
            {
                return (false, error.Message);

            }
        }

        private static async Task<(bool, string)> GetTmdbAccountID(string sessionID)
        {
            string tmdbBaseUrl = ConfigurationManager.AppSettings["TMDBApiBaseUrl"];
            string tmdbAuthToken = ConfigurationManager.AppSettings["TMDBNathanAuthToken"];

            string tmdbUrl = $"{tmdbBaseUrl}/account/account_id?session_id={sessionID}";


            // initialize client
            var client = new RestClient();

            // pass the url to request
            var request = new RestRequest(tmdbUrl);

            request.AddHeader("Authorization", $"Bearer {tmdbAuthToken}");

            // retrieve the response
            var response = await client.ExecuteAsync(request);

            try
            {
                if (response.IsSuccessful)
                {
                    // Deserialize
                    TMDB_Account accountInfo = JsonConvert.DeserializeObject<TMDB_Account>(response.Content);
                    if (accountInfo != null)
                    {
                        return (true, accountInfo.ID.ToString());
                    }
                    else
                    {
                        return (false, response.Content);
                    }
                }
                else
                {
                    return (false, response.Content);
                }
            }
            catch(Exception error) 
            {
                return (false, error.Message);
            }
        }

        /// <summary>
        /// Update the user's platform ID. Used when the user is allowed to update their api IDs (lastFM username, steamID, tmdb session ID).
        /// </summary>
        /// <param name="platformID">The ID for the platform (lastFM, steam, tmdb).</param>
        /// <param name="newUserPlatformID">The user's platform ID (lastFM username, steam ID, tmdb session ID)</param>
        /// <returns>(bool, string) bool to indicate success and string for the message (error/success).</returns>
        public static async Task<(bool, string)> UpdateUserPlatformID(int platformID, string newUserPlatformID)
        {
            // Retrieve the platform name, for display purposes.
            string platformName = GetPlatformName(platformID);

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

        // Method: Retrieve the platform name based on the ID.
        private static string GetPlatformName(int platformID)
        {
            string platformName = string.Empty;

            switch (platformID)
            {
                case STEAM_PLATFORM_ID:
                    platformName = "Steam";
                    break;
                case LASTFM_PLATFORM_ID:
                    platformName = "LastFM";
                    break;
                case TMDB_PLATFORM_ID:
                    platformName = "TMDB";
                    break;
                default:
                    break;
            }

            return platformName;
        }

        /// <summary>
        /// Favorites a media. Used for buttons that when clicked, favorites or unfavorites the media. The SP used, favorites/unfavorites the media.
        /// </summary>
        /// <param name="platformID">The ID of the platform (lastFM, steam, tmdb).</param>
        /// <param name="username">The username of the current logged in user.</param>
        /// <param name="mediaTypeID">The type of media (Game, Tv Show, Movie, etc.)</param>
        /// <param name="mediaPlatformID">The ID of that media in their corresponding platform. (Ex: app ID for steam games)</param>
        /// <param name="title">The title of the media.</param>
        /// <param name="album">If the media is music, the album.</param>
        /// <param name="artist">If the media is music, the artist.</param>
        /// <returns>(bool, string) bool indicates success and string is the message.</returns>
        public static async Task<(bool, string)> FavoriteMedia(int platformID, string username, MediaTypeID mediaTypeID, string mediaPlatformID = "", string title = "", string album = "", string artist = "")
        {
            // Ensure the username is not null or empty.
            if (string.IsNullOrEmpty(username))
            {
                return (false, "Username is null or empty.");
            }

            try
            {
                // Parameters for the SP.
                var parameters = new
                {
                    platform_id_input = platformID,
                    media_type_id_input = (int)mediaTypeID,
                    media_plat_id_input = mediaPlatformID,
                    title_input = title,
                    album_input = album,
                    artist_input = artist,
                    username_input = username
                };

                // Use SP to favorite/unfavorite media.
                var response = await _connection.Rpc(INITIAL_MEDIA_FAV_SP_NAME, parameters);

                var spResponse = response.Content;

                return (true, spResponse ?? "No message returned by the SP.");
            }
            catch (Exception error)
            {
                return (false, error.Message);
            }
        }

        /// <summary>
        /// Retrieve the list of favorites. Sorts the list to ensure that it only returns the list of favorites that the current logged in user has favorited and that the favorite field is not false.
        /// </summary>
        /// <returns>The list of media favorites. All types of media, not just games, etc.</returns>
        public static async Task<List<UserFavoriteMedia>> GetFavoriteMediaList()
        {
            // Favorites of all users.
            List<UserFavorite> userFavorites = await SupabaseConnection.GetTableRecord<UserFavorite>(_connection);

            // All media.
            List<Media> mediaList = await SupabaseConnection.GetTableRecord<Media>(_connection);

            // Create a new list where it only contains media that the current logged in user has favorited and that the favorite field is currently set to true.
            List<UserFavoriteMedia> result = (from media in mediaList
                                             join favorite in userFavorites
                                                on media.MediaID equals favorite.MediaID
                                             where favorite.Username == _username && favorite.Favorite
                                             select new UserFavoriteMedia
                                             {
                                                 MediaID = media.MediaID,
                                                 PlatformID = media.PlatformID,
                                                 MediaTypeID = media.MediaTypeID,
                                                 MediaPlatID = media.MediaPlatID,
                                                 Title = media.Title,
                                                 Album = media.Album,
                                                 Artist = media.Artist,
                                                 Username = favorite.Username,
                                                 IsFavorite = favorite.Favorite
                                             }).ToList();

            return result;
        }
    }
}
