using media_tracker_desktop.Models;
using media_tracker_desktop.Models.ApiModels;
using media_tracker_desktop.Models.SupabaseTables;
using media_tracker_desktop.Models.TMDB;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using Supabase;
using System.Configuration;


namespace media_tracker_desktop
{
    public static class UserAppAccount
    {
        // Stored Procedure Names
        
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

        public static Client GetConnection
        {
            get { return _connection; }
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

        public static void UpdateUserSessionVariables(User user)
        {
            if (!user.Username.IsNullOrEmpty()) { 
            _username = user.Username;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _email = user.Email;
            _userLoggedIn = true; }
        }

        public static async Task UpdateUserSessionVariables(User user, List<UserAccount> userAccounts)
        {
            UpdateUserSessionVariables(user);

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
                        var (accountIDFound, accountID) = await GetTmdbAccountID(userAccount.UserPlatID.ToString());

                        // Only update session variables for tmdb when both account and session are not null.
                        if (accountIDFound)
                        {
                            _userTmdbSessionID = userAccount.UserPlatID.ToString();
                            TmdbApi.SessionID = _userTmdbSessionID;

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


        // Method: Retrieve the user's platform ID based on the platform ID passed.
        public static string GetUserPlatformID(int platformID)
        {
            switch (platformID)
            {
                case STEAM_PLATFORM_ID:
                    return _userSteamID;
                case LASTFM_PLATFORM_ID:
                    return _userLastFmID;
                case TMDB_PLATFORM_ID:
                    return _userTmdbSessionID;
                default:
                    return "Invalid platform ID.";
            }
        }
        // Method: Log out the user from the specified platform.
        public static void LogOutPlatform(int platformID)
        {
            switch (platformID)
            {
                case STEAM_PLATFORM_ID:
                    _userSteamID = "";

                    SteamApi.Logout();
                    break;
                case LASTFM_PLATFORM_ID:
                    _userLastFmID = "";

                    LastFMApi.Logout();
                    break;
                case TMDB_PLATFORM_ID:
                    _userTmdbSessionID = "";
                    _userTmdbAccountID = "";

                    TmdbApi.Logout();
                    break;
                default:
                    break;
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

        // Method: Retrieve the platform name based on the ID.
        public static string GetPlatformName(int platformID)
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
