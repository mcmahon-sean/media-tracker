using media_tracker_desktop.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public static class UserAppAccount
    {
        // Stored Procedure Names
        private const string CREATE_USER_SP_NAME = "CreateUser";
        private const string AUTHENTICATE_USER_SP_NAME = "AuthenticateUser";

        private static Client _connection;

        private static bool _userLoggedIn = false;
        private static string _username = string.Empty;

        // Method: Returns bool if a user is logged in.
        public static bool UserLoggedIn
        {
            get { return _userLoggedIn; }
        }

        // Method: Returns the username of the logged in user.
        public static string Username
        {
            get { return _username; }
        }

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
        /// <param name="connection">Connection to the DB.</param>
        /// <param name="user"></param>
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
                    message = "Successfully logged in.";

                    // Update parameters to new user.
                    _username = user.Username;
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

        public static void LogOut()
        {
            _username = string.Empty;
            _userLoggedIn = false;
        }
    }
}
