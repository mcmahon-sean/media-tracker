using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    // This model represents the user json object returned by the user.getInfo endpoint of LastFM API.
    public class LastFM_User
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("age")]
        public int Age { get; }


        // If something goes wrong with this property, it is originally an int in the json but in this model it is a bool. It seems to convert (0, 1) to bool automatically so far.
        [JsonProperty("subscriber")]
        public bool IsSubscriber { get; }


        [JsonProperty("realname")]
        public string RealName { get; } = string.Empty;


        [JsonProperty("playcount")]
        public int PlayCount { get; }


        [JsonProperty("artist_count")]
        public int ArtistCount { get; }


        [JsonProperty("album_count")]
        public int AlbumCount { get; }


        [JsonProperty("track_count")]
        public int TrackCount { get; }


        [JsonProperty("country")]
        public string Country { get; } = string.Empty;


        [JsonProperty("gender")]
        public string Gender { get; } = string.Empty;


        [JsonProperty("url")]
        public string ProfileUrl { get; } = string.Empty;


        [JsonProperty("registered")]
        private LastFM_User_Registered Registered { get; }

        public DateTime RegisteredAt
        {
            get
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(Registered.UnixTime).UtcDateTime;
            }
        }


        // Just reusing the LastFM_Artist_Image since making a new one (LastFM_User_Image) would just be identical.
        // If there are any differences between the two, then a new one is recommended.
        [JsonProperty("image")]
        private List<LastFM_Artist_Image>? Image { get; }

        public string? ImageUrl
        {
            get
            {
                // If there is no list of images, return null.
                if (Image == null)
                {
                    return null;
                }
                // Else,
                else
                {
                    // Return the url of the last image that is not null or empty.
                    return Image.Where(image => !string.IsNullOrEmpty(image?.Text)).LastOrDefault()?.Text;
                }
            }
        }


        [JsonConstructor]
        public LastFM_User(string name, int age, int subscriber, string realName, int playCount, int artistCount, int albumCount, int trackCount, string country, string gender, string profileUrl, LastFM_User_Registered registered, List<LastFM_Artist_Image>? image)
        {
            this.Name = name;
            this.Age = age;
            this.IsSubscriber = subscriber == 1 ? true : false;
            this.RealName = realName;
            this.PlayCount = playCount;
            this.ArtistCount = artistCount;
            this.AlbumCount = albumCount;
            this.TrackCount = trackCount;
            this.Country = country;
            this.Gender = gender;
            this.ProfileUrl = profileUrl;
            this.Registered = registered;
            this.Image = image;
        }
    }
}
