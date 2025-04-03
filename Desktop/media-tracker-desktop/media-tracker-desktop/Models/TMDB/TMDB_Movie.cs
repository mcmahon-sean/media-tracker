using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.TMDB
{
    public class TMDB_Movie
    {
        [JsonProperty("id")]
        public int ID { get; }

        [JsonProperty("title")]
        public string Title { get; } = string.Empty;

        [JsonProperty("overview")]
        public string Overview { get; } = string.Empty;

        [JsonConstructor]
        public TMDB_Movie(int id, string title, string overview)
        {
            this.ID = id;
            this.Title = title;
            this.Overview = overview;
        }
    }
}
