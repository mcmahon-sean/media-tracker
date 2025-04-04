using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.TMDB
{
    // This model represents tv show object.
    public class TMDB_TV_Show
    {
        [JsonProperty("id")]
        public int ID { get; }

        [JsonProperty("name")]
        public string Name { get; } = string.Empty;

        [JsonProperty("overview")]
        public string Overview { get; } = string.Empty;

        [JsonConstructor]
        public TMDB_TV_Show(int id, string name, string overview)
        {
            this.ID = id;
            this.Name = name;
            this.Overview = overview;
        }
    }
}
