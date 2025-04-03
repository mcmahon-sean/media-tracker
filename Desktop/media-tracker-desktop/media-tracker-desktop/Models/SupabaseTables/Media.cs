using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("Media")]
    public class Media : BaseModel
    {
        [PrimaryKey("MediaID")]
        public int MediaID { get; set; }


        [Column("PlatformID")]
        public int PlatformID { get; set; }


        [Column("MediaTypeID")]
        public int MediaTypeID { get; set; }


        [Column("MediaPlatID")]
        public string MediaPlatID { get; set; } = string.Empty;


        [Column("Title")]
        public string Title { get; set; } = string.Empty;


        [Column("Album")]
        public string Album { get; set; } = string.Empty;


        [Column("Artist")]
        public string Artist { get; set; } = string.Empty;
    }
}
