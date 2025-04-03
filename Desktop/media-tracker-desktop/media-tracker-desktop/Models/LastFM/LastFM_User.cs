using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_User
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("age")]
        public int Age { get; }


        [JsonProperty("isSubscriber")]
        public bool IsSubscriber { get; }


        [JsonProperty("realName")]
        public string RealName { get; } = string.Empty;


        [JsonProperty("playCount")]
        public int PlayCount { get; }


        [JsonProperty("artistCount")]
        public int ArtistCount { get; }


        [JsonProperty("albumCount")]
        public int AlbumCount { get; }


        [JsonProperty("trackCount")]
        public int TrackCount { get; }


        [JsonProperty("country")]
        public string Country { get; } = string.Empty;


        [JsonProperty("gender")]
        public string Gender { get; } = string.Empty;


        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; } = string.Empty;


        [JsonProperty("registeredAt")]
        public DateTime RegisteredAt { get; }


        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; }


        [JsonConstructor]
        public LastFM_User(string name, int age, bool isSubscriber, string realName, int playCount, int artistCount, int albumCount, int trackCount, string country, string gender, string profileUrl, DateTime registeredAt, string? imageUrl)
        {
            this.Name = name;
            this.Age = age;
            this.IsSubscriber = isSubscriber;
            this.RealName = realName;
            this.PlayCount = playCount;
            this.ArtistCount = artistCount;
            this.AlbumCount = albumCount;
            this.TrackCount = trackCount;
            this.Country = country;
            this.Gender = gender;
            this.ProfileUrl = profileUrl;
            this.RegisteredAt = registeredAt;
            this.ImageUrl = imageUrl;
        }
    }
}
