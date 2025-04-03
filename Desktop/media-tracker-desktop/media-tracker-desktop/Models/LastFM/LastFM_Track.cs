using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Track
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("artist")]
        public string Artist { get; } = string.Empty;


        [JsonProperty("album")]
        public string Album { get; } = string.Empty;


        [JsonProperty("url")]
        public string Url { get; } = string.Empty;


        [JsonProperty("timestamp")]
        public int? Timestamp { get; }


        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; }


        [JsonConstructor]
        public LastFM_Track(string name, string artist, string album, string url, int? timestamp, string? imageUrl)
        {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.Url = url;
            this.Timestamp = timestamp;
            this.ImageUrl = imageUrl;
        }
    }
}
