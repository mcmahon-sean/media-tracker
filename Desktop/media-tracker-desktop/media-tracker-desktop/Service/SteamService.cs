using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using media_tracker_desktop.Models.Steam;

namespace media_tracker_desktop.Services
{
    public class SteamService
    {
        private readonly string _apiKey = ConfigurationManager.AppSettings["SteamAPIKey"];
        private readonly string _baseUrl = ConfigurationManager.AppSettings["SteamApiBaseUrl"];

        //public async Task<List<Steam_Game>> GetOwnedGamesAsync(string steamId)
        //{
        //    var client = new RestClient();
        //    var url = $"{_baseUrl}key={_apiKey}&steamid={steamId}&include_appinfo=1&format=json";
        //    var req = new RestRequest(url, Method.Get);
        //    var resp = await client.ExecuteAsync(req);
        //    if (!resp.IsSuccessful)
        //        throw new Exception($"Steam fetch failed ({resp.StatusCode}): {resp.Content}");

        //    var root = JsonConvert.DeserializeObject<SteamRoot>(resp.Content);
        //    return root?.Response?.Games ?? new List<Steam_Game>();
        //}
    }

    //public class SteamOwnedGamesResponse
    //{
    //    [JsonProperty("game_count")]
    //    public int GameCount { get; set; }

    //    [JsonProperty("games")]
    //    public List<Steam_Model> Games { get; set; }
    //}

    //public class SteamRoot
    //{
    //    [JsonProperty("response")]
    //    public SteamOwnedGamesResponse Response { get; set; }
    //}
}
