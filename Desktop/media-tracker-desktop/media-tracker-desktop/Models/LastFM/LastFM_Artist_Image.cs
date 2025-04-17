using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.LastFM
{
    public class LastFM_Artist_Image
    {
        [JsonProperty("#text")]
        public string Text { get; } = string.Empty;

        public LastFM_Artist_Image(string text)
        {
            this.Text = text;
        }
    }
}
