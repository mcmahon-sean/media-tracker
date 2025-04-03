using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Artist
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("playCount")]
        public int PlayCount { get; }


        [JsonProperty("url")]
        public string Url { get; } = string.Empty;


        [JsonProperty("mbid")]
        public string Mbid { get; } = string.Empty;


        [JsonProperty("rank")]
        public int Rank { get; }


        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; }


        [JsonConstructor]
        public LastFM_Artist(string name, int playCount, string url, string mbid, int rank, string? imageUrl)
        {
            this.Name = name;
            this.PlayCount = playCount;
            this.Url = url;
            this.Mbid = mbid;
            this.Rank = rank;
            this.ImageUrl = imageUrl;
        }
    }
}
