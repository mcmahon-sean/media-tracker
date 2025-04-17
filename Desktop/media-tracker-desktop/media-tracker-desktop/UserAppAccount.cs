using media_tracker_desktop.Models;
using media_tracker_desktop.Models.SupabaseTables;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using Supabase;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public static class UserAppAccount
    {
        // Stored Procedure Names
        private const string CREATE_USER_SP_NAME = "CreateUser";
        private const string AUTHENTICATE_USER_SP_NAME = "AuthenticateUser";
        private const string ADD_THIRD_PARTY_ID = "add_3rd_party_id";
        private const int STEAM_PLATFORM_ID = 1;
        private const int LASTFM_PLATFORM_ID = 2;
        private const int TMDB_PLATFORM_ID = 3;

        private static Client _connection;

        private static bool _userLoggedIn = false;

        private static string _username = string.Empty;
        private static string _firstName = string.Empty;
        private static string _lastName = string.Empty;
        private static string _email = string.Empty;

        private static string _userLastFmID = string.Empty;
        private static string _userSteamID = string.Empty;
        private static string _userTmdbID = string.Empty;
        // ----- Getter Methods -----
        // Method: Returns bool if a user is logged in.
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

        //Getter Methods for the Platform Ids because I didnt want to memorize them :P
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

        // Method: Returns the user's LastFM id.
        public static string UserLastFmID
        {
            get { return _userLastFmID; }
        }

        // Method: Returns the user's Steam id.
        public static string UserSteamID
        {
            get { return _userSteamID; }
        }

        // Method: Returns the user's TMDB id.
        public static string UserTmdbID
        {
            get { return _userTmdbID; }
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
                    UpdateUserSessionVariables(userQueryResult.Model!, userAccountQueryResult.Models);

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
        /// </summary>
        /// <param name="user">The user object of the logged in user.</param>
        /// <param name="userAccounts">The list of user account objects that is associated with the user.</param>
        private static void UpdateUserSessionVariables(User user, List<UserAccount> userAccounts)
        {
            _username = user.Username;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _email = user.Email;

            // Foreach platform account that the user has,
            foreach (UserAccount userAccount in userAccounts)
            {
                // Determine the platform and store that user's id for that platform.
                switch (userAccount.PlatformID)
                {
                    case STEAM_PLATFORM_ID:
                        _userSteamID = userAccount.UserPlatID.ToString();
                        break;
                    case LASTFM_PLATFORM_ID:
                        _userLastFmID = userAccount.UserPlatID.ToString();
                        break;
                    case TMDB_PLATFORM_ID:
                        _userTmdbID = userAccount.UserPlatID.ToString();
                        break;
                    default:
                        break;
                }
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
            _userTmdbID = string.Empty;
        }

        public static async Task<(bool, string)> AddThirdPartyId(int? PlatformId, string UserPlatformId)
        {
            var parameters = new{
                username_input = _username,
                platform_id_input = PlatformId,
                user_plat_id_input = UserPlatformId
            };
            if (!PlatformId.HasValue)
            {
                return (false, "Platform ID is null.");
            }
            if (_connection == null)
            {
                return (false, "Connection is null.");
            }
            try{
                var response = await _connection.Rpc(ADD_THIRD_PARTY_ID, parameters);
                string returnedString = response.Content.ToString();
                if (returnedString.IsNullOrEmpty()){
                    return (false, "Database did not return a string value.");
                }
                return (true, returnedString);
            }
            catch (Exception error)
            {
                return (false, error.Message);

            }
        }
    }
}
