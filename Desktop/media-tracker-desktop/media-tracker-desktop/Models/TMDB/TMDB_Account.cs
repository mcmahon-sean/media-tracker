using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.TMDB
{
    public class TMDB_Account
    {
        [JsonProperty("username")]
        public string Username { get; } = string.Empty;

        [JsonProperty("id")]
        public int ID { get; }

        [JsonConstructor]
        public TMDB_Account(string username, int id)
        {
            this.Username = username;
            this.ID = id;
        }
    }
}
