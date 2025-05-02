using media_tracker_desktop.Models.LastFM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace media_tracker_desktop.Models.ApiModels
{
    public static class LastFMApi
    {
        private const string CONFIG_KEY_FOR_LASTFM_BASE_URL = "LastFMApiBaseUrl";
        private const string CONFIG_KEY_FOR_LASTFM_API_KEY = "LastFMApiKey";

        private static string _baseUrl = string.Empty;
        private static string _apiKey = string.Empty;
        private static string _user = string.Empty;
        private static RestClient _client = new RestClient();

        // Property: User
        public static string User
        {
            // Retrieve the lastFM username.
            get { return _user; }
            // Set the lastFM username.
            set { _user = value; }
        }

        /// <summary>
        /// Initializes the LastFMApi class so that it is ready to work with the LastFM api. 
        /// </summary>
        /// <exception cref="Exception">Throws exception when base url and/or api key is null. Rethrows any exception that is thrown when retrieving information from configuration.</exception>
        public static void Initialize()
        {
            string? baseUrl;
            string? apiKey;

            // Ensure no exception occurs when retrieving the properties from ConfigurationManager.
            try
            {
                baseUrl = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_LASTFM_BASE_URL];
                apiKey = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_LASTFM_API_KEY];

                // Ensure base url and api key are not null.
                // Otherwise, throws exception.
                if (!string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(apiKey))
                {
                    _baseUrl = baseUrl;
                    _apiKey = apiKey;
                }
                else
                {
                    throw new Exception("Base url and/or api key is null or empty.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if the properties are valid.
        /// </summary>
        /// <returns>A boolean indicating if the properties are valid and a message for any errors.</returns>
        private static (bool, string) CheckRequiredProperties()
        {
            bool isValid = true;
            string message = "";

            if (string.IsNullOrEmpty(_baseUrl))
            {
                isValid = false;
                message += "Base url is null or empty.\n";
            }
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                isValid = false;
                message += "Api key is null or empty.\n";
            }
            
            if (string.IsNullOrEmpty(_user))
            {
                isValid = false;
                message += "User is null or empty.\n";
            }

            return (isValid, message);
        }

        /// <summary>
        /// Retrieve the user's list of top artists returned by the user.getTopArtists LastFM api endpoint. Execute Initialize() and set User property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the list of artists.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, List<LastFM_Artist>?)> GetUserTopArtists()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties();

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            RestRequest request = new RestRequest(_baseUrl);

            request.AddParameter("method", "user.getTopArtists");
            request.AddParameter("user", _user);
            request.AddParameter("api_key", _apiKey);
            request.AddParameter("limit", 5);
            request.AddParameter("format", "json");

            var response = await _client.ExecuteAsync(request);

            // If response is not successful, throw exception.
            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }
            // If the response has no content, return empty list.
            else if (string.IsNullOrEmpty(response.Content))
            {
                return (false, []);
            }
            else
            {
                // Convert content to json object.
                var topArtistsJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var artistsJson = topArtistsJson.Root["topartists"]["artist"];

                // Deserialize
                List<LastFM_Artist>? artists = JsonConvert.DeserializeObject<List<LastFM_Artist>>(artistsJson.ToString());

                return (true, artists);
            }
        }

        /// <summary>
        /// Retrieve the user's list of recent tracks returned by the user.getRecentTracks LastFM api endpoint. Execute Initialize() and set User property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the list of tracks.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, List<LastFM_Track>?)> GetUserRecentTracks()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties();

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            RestRequest request = new RestRequest(_baseUrl);

            request.AddParameter("method", "user.getRecentTracks");
            request.AddParameter("user", _user);
            request.AddParameter("api_key", _apiKey);
            request.AddParameter("limit", 5);
            request.AddParameter("format", "json");

            var response = await _client.ExecuteAsync(request);

            // If response is not successful, throw exception.
            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }
            // If the response has no content, return empty list.
            else if (string.IsNullOrEmpty(response.Content))
            {
                return (false, []);
            }
            else
            {
                // Convert content to json object.
                var recentTracksJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var tracksJson = recentTracksJson.Root["recenttracks"]["track"];

                // Deserialize
                List<LastFM_Track> tracks = JsonConvert.DeserializeObject<List<LastFM_Track>>(tracksJson.ToString());

                return (true, tracks);
            }
        }

        /// <summary>
        /// Retrieve the user's info returned by the user.getInfo LastFM api endpoint. Execute Initialize() and set User property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the user object.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, LastFM_User?)> GetUserInfo()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties();

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            RestRequest request = new RestRequest(_baseUrl);

            request.AddParameter("method", "user.getInfo");
            request.AddParameter("user", _user);
            request.AddParameter("api_key", _apiKey);
            request.AddParameter("format", "json");

            var response = await _client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (false, null);
            }

            // If response is not successful, throw exception.
            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }
            // If the response has no content, return empty list.
            else if (string.IsNullOrEmpty(response.Content))
            {
                return (false, null);
            }
            else
            {
                // Convert content to json object.
                var userInfoJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var userJson = userInfoJson.Root["user"];

                // Deserialize
                LastFM_User userInfo = JsonConvert.DeserializeObject<LastFM_User>(userJson.ToString());

                return (true, userInfo);
            }
        }

        public static void Logout()
        {
            _user = string.Empty;
        }
    }
}
