using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using media_tracker_desktop.Models.TMDB;

namespace media_tracker_desktop.Services
{
    public class TmdbService
    {
        private readonly string _apiKey = ConfigurationManager.AppSettings["TMDBApiKey"]!;
        private const string _baseUrl = "https://api.themoviedb.org/3/account";

        public async Task<TMDB_Account> GetAccountDetailsAsync(string sessionId)
        {
            var url = $"{_baseUrl}?api_key={_apiKey}&session_id={sessionId}";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            var resp = await client.ExecuteAsync(request);
            if (!resp.IsSuccessful)
                throw new Exception($"TMDB fetch failed: {resp.Content}");

            return JsonConvert.DeserializeObject<TMDB_Account>(resp.Content)
                   ?? throw new Exception("Invalid TMDB response");
        }
    }
}
