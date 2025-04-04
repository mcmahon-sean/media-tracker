using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.Steam
{
    // This model represents each game object in the games array in the response json object returned by the GetOwnedGames endpoint of the SteamAPI.
    public class Steam_Model
    {
        [JsonProperty("appid")]
        public int AppID { get; }


        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("playtime_forever")]
        public int PlaytimeForever { get; }


        [JsonProperty("img_icon_url")]
        public string ImgIconUrl { get; }


        [JsonProperty("has_community_visible_stats")]
        public bool HasCommunityStats { get; }


        [JsonProperty("playtime_windows_forever")]
        public int PlaytimeWindows { get; }


        [JsonProperty("playtime_mac_forever")]
        public int PlaytimeMac { get; }


        [JsonProperty("playtime_linux_forever")]
        public int PlaytimeLinux { get; }


        [JsonProperty("playtime_deck_forever")]
        public int PlaytimeDeck { get; }


        [JsonProperty("rtime_last_played")]
        public int RTimeLastPlayed { get; }


        [JsonProperty("playtime_disconnected")]
        public int PlaytimeDisconnected { get; }


        [JsonConstructor]
        public Steam_Model(int appId, string name, int playtimeForever, string imgIconUrl, bool hasCommunityStats, int playtimeWindows, int playtimeMac, int playtimeLinux, int playtimeDeck, int rtimeLastPlayed, int playtimeDisconnected)
        {
            this.AppID = appId;
            this.Name = name;
            this.PlaytimeForever = playtimeForever;
            this.ImgIconUrl = imgIconUrl;
            this.HasCommunityStats = hasCommunityStats;
            this.PlaytimeWindows = playtimeWindows;
            this.PlaytimeMac = playtimeMac;
            this.PlaytimeLinux = playtimeLinux;
            this.PlaytimeDeck = playtimeDeck;
            this.RTimeLastPlayed = rtimeLastPlayed;
            this.PlaytimeDisconnected = playtimeDisconnected;
        }
    }
}
