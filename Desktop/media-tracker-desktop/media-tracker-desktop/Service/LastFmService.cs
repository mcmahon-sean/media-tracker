using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using media_tracker_desktop.Models.LastFM;

namespace media_tracker_desktop.Services
{
    public class LastFmService
    {
        private readonly string _apiKey  = ConfigurationManager.AppSettings["LastFMApiKey"];
        private readonly string _baseUrl = ConfigurationManager.AppSettings["LastFMApiBaseUrl"];

        public async Task<LastFM_User> GetUserInfoAsync(string username)
        {
            var client  = new RestClient(_baseUrl);
            var request = new RestRequest();
            request.AddParameter("method",    "user.getinfo");
            request.AddParameter("user",      username);
            request.AddParameter("api_key",   _apiKey);
            request.AddParameter("format",    "json");

            var resp = await client.ExecuteAsync(request);
            if (!resp.IsSuccessful)
                throw new Exception($"Last.fm fetch failed: {resp.ErrorMessage}");

            var wrapper = JsonConvert.DeserializeObject<LastFmUserResponse>(resp.Content);
            return wrapper?.User ?? throw new Exception("Invalid LastFM response");
        }
    }

    internal class LastFmUserResponse
    {
        [JsonProperty("user")]
        public LastFM_User User { get; set; }
    }
}
