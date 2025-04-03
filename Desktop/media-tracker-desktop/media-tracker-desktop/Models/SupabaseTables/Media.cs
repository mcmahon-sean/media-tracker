using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("media")]
    public class Media : BaseModel
    {
        [PrimaryKey("media_id")]
        public int MediaID { get; set; }


        [Column("platform_id")]
        public int PlatformID { get; set; }


        [Column("media_type_id")]
        public int MediaTypeID { get; set; }


        [Column("media_plat_id")]
        public string MediaPlatID { get; set; } = string.Empty;


        [Column("title")]
        public string Title { get; set; } = string.Empty;


        [Column("album")]
        public string Album { get; set; } = string.Empty;


        [Column("artist")]
        public string Artist { get; set; } = string.Empty;
    }
}
