using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Track_Album
    {
        [JsonProperty("#text")]
        public string Text { get; }

        public LastFM_Track_Album(string text)
        {
            this.Text = text;
        }
    }
}
