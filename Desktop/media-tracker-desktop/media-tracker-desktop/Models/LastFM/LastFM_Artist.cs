using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    // This model represents each artist object in the artist array of the topartists json object returned by the user.getTopArtists endpoint of LastFM API.
    public class LastFM_Artist
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("playcount")]
        public int PlayCount { get; }


        [JsonProperty("url")]
        public string Url { get; } = string.Empty;


        [JsonProperty("mbid")]
        public string Mbid { get; } = string.Empty;


        [JsonProperty("@attr")]
        private LastFM_Artist_Attr Attr { get; }

        public int Rank
        {
            get
            {
                return Attr.Rank;
            }
        }


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
        public LastFM_Artist(string name, int playCount, string url, string mbid, LastFM_Artist_Attr attr, List<LastFM_Artist_Image>? image)
        {
            this.Name = name;
            this.PlayCount = playCount;
            this.Url = url;
            this.Mbid = mbid;
            this.Attr = attr;
            this.Image = image;
        }
    }
}
