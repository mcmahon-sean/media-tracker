using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    // This model represents each track object in the track array in the recenttracks json object returned by the user.getRecentTracks endpoint of LastFM API.
    public class LastFM_Track
    {
        [JsonProperty("name")]
        public string Name { get; } = string.Empty;


        [JsonProperty("artist")]
        private LastFM_Track_Artist Artist { get; }

        public string ArtistName
        {
            get
            {
                return Artist.Text;
            }
        }


        [JsonProperty("album")]
        private LastFM_Track_Album Album { get; }

        public string AlbumName
        {
            get
            {
                return Album.Text;
            }
        }


        [JsonProperty("url")]
        public string Url { get; } = string.Empty;


        [JsonProperty("date")]
        private LastFM_Track_Date? Date { get; }

        public int? Timestamp
        {
            get
            {
                // If there is no list of images, return null.
                if (Date == null)
                {
                    return null;
                }
                // Else,
                else
                {
                    return Date.Uts;
                }
            }
        }


        // Just reusing the LastFM_Artist_Image since making a new one (LastFM_Track_Image) would just be identical.
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
        public LastFM_Track(string name, LastFM_Track_Artist artist, LastFM_Track_Album album, string url, LastFM_Track_Date? date, List<LastFM_Artist_Image>? image)
        {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.Url = url;
            this.Date = date;
            this.Image = image;
        }
    }
}
