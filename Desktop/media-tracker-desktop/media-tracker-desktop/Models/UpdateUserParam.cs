using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    // This class is used as an object parameter passed to the update_user stored procedure in supabase.
    // The argument in JsonProperty is the parameter name, as specified in the data dictionary.
    // Throws an exception if the value passed is not valid.
    public class UpdateUserParam
    {
        [JsonProperty("username_input")]
        public string Username { get; }

        [JsonProperty("first_name_input")]
        public string FirstName { get; }

        [JsonProperty("last_name_input")]
        public string LastName { get; }

        [JsonProperty("email_input")]
        public string Email { get; }

        [JsonProperty("password_input")]
        public string Password { get; }

        public UpdateUserParam(string username, string firstName, string lastName, string email, string password)
        {
            this.Username = ValidateUsername(username);
            this.FirstName = EnsureFieldNotNullOrEmpty("First Name", firstName);
            this.LastName = lastName;
            this.Email = ValidateEmail(email);
            this.Password = EnsureFieldNotNullOrEmpty("Password", password);
        }

        // Method: Validates the username field.
        private string ValidateUsername(string username)
        {
            // Make sure it is not null or empty.
            string usernameNotNullOrEmpty = EnsureFieldNotNullOrEmpty("Username", username);

            // Make sure that it is no larger than 15 characters.
            if (usernameNotNullOrEmpty.Length > 15)
            {
                throw new Exception("Please enter a username that is no larger than 15 characters.");
            }
            else
            {
                return username;
            }
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

        // Method: Throws an exception if an email doesn't contain "@" or ".".
        private string ValidateEmail(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new ArgumentException("Email must be a valid email address.");
            }

            return email;
        }
    }
}
