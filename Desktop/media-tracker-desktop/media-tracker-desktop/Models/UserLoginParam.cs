using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    // This class is used as an object parameter passed to the AuthenticateUser stored procedure in supabase.
    // The argument in JsonProperty is the parameter name, as specified in the data dictionary.
    // Throws an exception if the value passed is not valid.
    public class UserLoginParam
    {
        [JsonProperty("usernamevar")]
        public string Username { get; }

        [JsonProperty("passwordvar")]
        public string Password { get; }

        public UserLoginParam(string username, string password)
        {
            this.Username = EnsureFieldNotNullOrEmpty("Username", username);
            this.Password = EnsureFieldNotNullOrEmpty("Password", password);
        }

        // Method: Throws an exception if the field is null or empty.
        private string EnsureFieldNotNullOrEmpty(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                throw new Exception($"Please enter a value for {fieldName}.");
            }

            return fieldValue;
        }
    }
}
