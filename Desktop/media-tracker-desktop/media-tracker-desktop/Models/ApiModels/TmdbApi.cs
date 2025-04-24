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

            //if (string.IsNullOrEmpty(_))
            //{
            //    isValid = false;
            //    message += "User is null or empty.\n";
            //}

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
    }
}
