using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_User_Registered
    {
        [JsonProperty("unixtime")]
        public int UnixTime { get; }

        public LastFM_User_Registered(int unixTime)
        {
            this.UnixTime = unixTime;
        }
    }
}
