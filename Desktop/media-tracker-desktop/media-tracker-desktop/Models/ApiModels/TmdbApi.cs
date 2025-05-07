using media_tracker_desktop.Models.Steam;
using media_tracker_desktop.Models.TMDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.ApiModels
{
    public static class TmdbApi
    {
        private const string CONFIG_KEY_FOR_TMDB_BASE_URL = "TMDBApiBaseUrl";
        private const string CONFIG_KEY_FOR_TMDB_API_KEY = "TMDBApiKey";
        private const string CONFIG_KEY_FOR_TMDB_AUTH_TOKEN = "TMDBNathanAuthToken";
        private const string CONFIG_KEY_FOR_TMDB_REQUEST_TOKEN_URL = "TMDBApiRequestTokenUrl";

        private static string _baseUrl = string.Empty;
        private static string _apiKey = string.Empty;
        private static string _authToken = string.Empty;
        private static string _requestTokenUrl = string.Empty;

        private static string _sessionID = string.Empty;
        private static string _accountID = string.Empty;
        private static RestClient _client = new RestClient();

        // Property: SessionID
        public static string SessionID
        {
            // Retrieve the sessionID.
            get { return _sessionID; }
            // Set the sessionID.
            set { _sessionID = value; }
        }

        // Property: AccountID
        public static string AccountID
        {
            // Retrieve the accountID.
            get { return _accountID; }
            // Set the accountID.
            set { _accountID = value; }
        }

        /// <summary>
        /// Initializes the TmdbApi class so that it is ready to work with the TMDB api. 
        /// </summary>
        /// <exception cref="Exception">Throws exception when base url, api key, auth token, and/or request token url is null. Rethrows any exception that is thrown when retrieving information from configuration.</exception>
        public static void Initialize()
        {
            string? baseUrl;
            string? apiKey;
            string? authToken;
            string? requestTokenUrl;

            // Ensure no exception occurs when retrieving the properties from ConfigurationManager.
            try
            {
                baseUrl = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_TMDB_BASE_URL];
                apiKey = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_TMDB_API_KEY];
                authToken = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_TMDB_AUTH_TOKEN];
                requestTokenUrl = ConfigurationManager.AppSettings[CONFIG_KEY_FOR_TMDB_REQUEST_TOKEN_URL];


                // Ensure base url and api key are not null.
                // Otherwise, throws exception.
                if (!string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(requestTokenUrl))
                {
                    _baseUrl = baseUrl;
                    _apiKey = apiKey;
                    _authToken = authToken;
                    _requestTokenUrl = requestTokenUrl;
                }
                else
                {
                    throw new Exception("Base url, api key, auth token, and/or request token url is null or empty.");
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
        private static (bool, string) CheckRequiredProperties(bool checkRequestTokenUrl = true, bool checkAccountID = true)
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

            if (string.IsNullOrEmpty(_authToken))
            {
                isValid = false;
                message += "Auth token is null or empty.\n";
            }

            if (string.IsNullOrEmpty(_requestTokenUrl) && checkRequestTokenUrl)
            {
                isValid = false;
                message += "Request token url is null or empty.\n";
            }

            if (string.IsNullOrEmpty(_sessionID))
            {
                isValid = false;
                message += "SessionID is null or empty.\n";
            }

            if (string.IsNullOrEmpty(_accountID) && checkAccountID)
            {
                isValid = false;
                message += "AccountID is null or empty.\n";
            }

            return (isValid, message);
        }

        public static async Task<string?> RetrieveSessionID()
        {
            // Build the url to create a new request token.
            string urlWithEndpoint = $"{_baseUrl}/authentication/token/new";
            string authHeader = $"Bearer {_authToken}";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", authHeader);

            var response = await _client.GetAsync(request);

            // If success,
            if (response.IsSuccessful)
            {
                // Retrieve the request token.
                var requestToken = JObject.Parse(response.Content)["request_token"].ToString();

                Console.WriteLine("{0}", response.Content);

                // Build the url to send the user to authenticate this app, appending the request token.
                var requestTokenUrl = $"{_requestTokenUrl}{requestToken}";

                // System.Diagnostics.Process is used to open the authentication in a new window.
                System.Diagnostics.Process.Start(new ProcessStartInfo(requestTokenUrl)
                {
                    UseShellExecute = true
                });

                DialogResult result = MessageBox.Show("Once you have authorized the app, press continue.", "Continue?", MessageBoxButtons.OK);

                //Response if a guarenteed failure if user doesnt hit approve before hitting ok
                if (result == DialogResult.OK)
                {
                    try
                    {
                        string jsonBodyToken = requestToken;

                        // Build the url to exchange the request token for a session token.
                        urlWithEndpoint = $"{_baseUrl}/authentication/session/new";

                        request = new RestRequest(urlWithEndpoint);

                        request.AddHeader("accept", "application/json");
                        request.AddHeader("Authorization", authHeader);
                        request.AddJsonBody(new { request_token = requestToken });

                        response = await _client.PostAsync(request);

                        var success = JObject.Parse(response.Content)["success"].ToString();

                        if (success == "True")
                        {
                            string sessionID = JObject.Parse(response.Content)["session_id"].ToString();

                            return sessionID;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Retrieve the user's account details returned by the TMDB api. Execute Initialize() and set SessionID property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the tmbd account object.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, TMDB_Account?)> GetAccountDetails()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties(checkRequestTokenUrl: false, checkAccountID: false);

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            // Add the endpoint to the base url.
            string urlWithEndpoint = $"{_baseUrl}/account/account_id";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddParameter("session_id", _sessionID);
            request.AddHeader("Authorization", $"Bearer {_authToken}");

            var response = await _client.ExecuteAsync(request);

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
                // Deserialize
                TMDB_Account account = JsonConvert.DeserializeObject<TMDB_Account>(response.Content)!;

                return (true, account);
            }
        }

        /// <summary>
        /// Retrieve the user's account details returned by the rated/movies TMDB api endpoint. Execute Initialize() and set AccountID and SessionID property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the list of movies.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, List<TMDB_Movie>?)> GetUserRatedMovies()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties(checkRequestTokenUrl: false);

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            // Add the endpoint to the base url.
            string urlWithEndpoint = $"{_baseUrl}/account/{_accountID}/rated/movies";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);
            request.AddParameter("sort_by", "created_at.desc");
            request.AddParameter("session_id", _sessionID);
            request.AddHeader("Authorization", $"Bearer {_authToken}");

            var response = await _client.ExecuteAsync(request);

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
                var movieResultJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var movieJson = movieResultJson.Root["results"];

                // Deserialize
                List<TMDB_Movie> movies = JsonConvert.DeserializeObject<List<TMDB_Movie>>(movieJson.ToString());

                return (true, movies);
            }
        }

        /// <summary>
        /// Retrieve the user's account details returned by the favorite/tv TMDB api endpoint. Execute Initialize() and set AccountID and SessionID property before using this method.
        /// </summary>
        /// <returns>A boolean indicating if the process is successful and the list of tv shows.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(bool, List<TMDB_TV_Show>?)> GetUserFavoriteTV()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties(checkRequestTokenUrl: false);

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            // Add the endpoint to the base url.
            string urlWithEndpoint = $"{_baseUrl}/account/{_accountID}/favorite/tv";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);
            request.AddParameter("sort_by", "created_at.asc");
            request.AddParameter("session_id", _sessionID);
            request.AddHeader("Authorization", $"Bearer {_authToken}");

            var response = await _client.ExecuteAsync(request);

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
                var tvShowResultJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var tvShowJson = tvShowResultJson.Root["results"];

                // Deserialize
                List<TMDB_TV_Show> tvShows = JsonConvert.DeserializeObject<List<TMDB_TV_Show>>(tvShowJson.ToString());

                return (true, tvShows);
            }
        }

        public static async Task<(bool, List<TMDB_Movie>?)> GetUserFavoriteMovies()
        {
            // Check necessary properties.
            (bool isValid, string message) propertyCheck = CheckRequiredProperties(checkRequestTokenUrl: false);

            if (!propertyCheck.isValid)
            {
                throw new Exception(propertyCheck.message);
            }

            // Add the endpoint to the base url.
            string urlWithEndpoint = $"{_baseUrl}/account/{_accountID}/favorite/movies";

            RestRequest request = new RestRequest(urlWithEndpoint);

            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);
            request.AddParameter("sort_by", "created_at.asc");
            request.AddParameter("session_id", _sessionID);
            request.AddHeader("Authorization", $"Bearer {_authToken}");

            var response = await _client.ExecuteAsync(request);

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
                var movieResultJson = JObject.Parse(response.Content);

                // Retrieve the array of artist from the json object and convert it back to string.
                var movieJson = movieResultJson.Root["results"];

                // Deserialize
                List<TMDB_Movie> movies = JsonConvert.DeserializeObject<List<TMDB_Movie>>(movieJson.ToString());

                return (true, movies);
            }
        }

        public static void Logout()
        {
            _accountID = string.Empty;
            _sessionID = string.Empty;
        }
    }
}
