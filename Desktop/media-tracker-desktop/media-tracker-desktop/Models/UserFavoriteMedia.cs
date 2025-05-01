using media_tracker_desktop.Models.LastFM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    // This model is used to combine the media and userFavorite table together,
    // which makes it easier to retrieve the details of the media that is a favorite.
    public class UserFavoriteMedia
    {
        public int MediaID { get; set; }

        public int PlatformID { get; set; }

        public int MediaTypeID { get; set; }

        public string MediaPlatID { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Album { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public bool IsFavorite { get; set; }
    }
}
