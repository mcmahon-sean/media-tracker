using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Artist_Attr
    {
        [JsonProperty("rank")]
        public int Rank { get; }

        public LastFM_Artist_Attr(int rank)
        {
            this.Rank = rank;
        }
    }
}
