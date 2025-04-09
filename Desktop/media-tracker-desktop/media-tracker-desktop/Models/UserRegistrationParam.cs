using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    // This class is used as an object parameter passed to the CreateUser stored procedure in supabase.
    // The argument in JsonProperty is the parameter name, as specified in the data dictionary.
    // Throws an exception if the value passed is not valid.
    public class UserRegistrationParam
    {
        [JsonProperty("usernamevar")]
        public string Username { get; }

        [JsonProperty("firstnamevar")]
        public string FirstName { get; }

        [JsonProperty("lastnamevar")]
        public string LastName { get; }

        [JsonProperty("emailvar")]
        public string Email { get; }

        [JsonProperty("passwordvar")]
        public string Password { get; }

        public UserRegistrationParam(string username, string firstName, string lastName, string email, string password)
        {
            this.Username = EnsureFieldNotNullOrEmpty("Username", username);
            this.FirstName = EnsureFieldNotNullOrEmpty("First Name", firstName);
            this.LastName = lastName;
            this.Email = ValidateEmail(email);
            this.Password = EnsureFieldNotNullOrEmpty("Password", password);
        }

        // Method: Throws an exception if the field is null or empty.
        private string EnsureFieldNotNullOrEmpty(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                throw new ArgumentNullException($"{fieldName} must not be null or empty.");
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
