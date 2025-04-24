using media_tracker_desktop.Models.LastFM;
using media_tracker_desktop.Models.Steam;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.ApiModels
{
    public static class SteamApi
    {
        private const string CONFIG_KEY_FOR_STEAM_BASE_URL = "SteamApiBaseUrl";
        private const string CONFIG_KEY_FOR_STEAM_API_KEY = "SteamApiKey";

        private static string _baseUrl = string.Empty;
        private static string _apiKey = string.Empty;
        private static string _steamID = string.Empty;
        private static RestClient _client = new RestClient();

        // Property: SteamID
        public static string SteamID
        {
            // Retrieve the user's steam ID.
            get { return _steamID; }
            // Set the user's steam ID.
            set
            {
                // Make sure it is not null or empty,
                // otherwise, throws exception.
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("SteamID is null or empty.");
                }
                else
                {
                    _steamID = value;
                }
            }
        }

        /// <summary>
        /// Initializes the SteamApi class so that it is ready to work with the Steam api.
        /// </summary>
        /// <exception cref="Exception">Throws exception when base url and/or api key is null. Rethrows any exception that is thrown when retrieving information from configuration.</exception>
        public static void Initialize()
        {
            string? baseUrl;
            string? apiKey;

            // Ensure no exception occurs when retrieving the properties from ConfigurationManager.
            try
            {
                baseUrl = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_STEAM_BASE_URL];
                apiKey = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_STEAM_API_KEY];

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
                isValid = true;
                message += "Api key is null or empty.\n";
            }

            if (string.IsNullOrEmpty(_steamID))
            {
                isValid = false;
                message += "SteamID is null or empty.\n";
            }

            return (isValid, message);
        }

        /// <summary>
        /// Retrieve the user's list of owned games returned by the GetOwnedGames Steam api endpoint. Execute Initialize() and set SteamID property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the list of games.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, List<Steam_Game>?)> GetOwnedGames()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties();

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            // Add the endpoint to the base url.
            string urlWithEndpoint = _baseUrl + "IPlayerService/GetOwnedGames/v0001/";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddParameter("key", _apiKey);
            request.AddParameter("steamid", _steamID);
            request.AddParameter("include_appinfo", 1);
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
                // Convert the content to a json object.
                var responseJson = JObject.Parse(response.Content);

                // Retrieve the games property of the json object and convert it back to a json string.
                var gamesJson = responseJson.Root["response"]["games"];

                // Deserialize
                List<Steam_Game>? games = JsonConvert.DeserializeObject<List<Steam_Game>>(gamesJson.ToString());

                return (true, games);
            }
        }
    }
}
