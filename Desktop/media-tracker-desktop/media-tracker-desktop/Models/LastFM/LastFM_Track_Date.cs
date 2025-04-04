using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Track_Date
    {
        [JsonProperty("uts")]
        public int Uts { get; }

        public LastFM_Track_Date(int uts)
        {
            this.Uts = uts;
        }
    }
}
