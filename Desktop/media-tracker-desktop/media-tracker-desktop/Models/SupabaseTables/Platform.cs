using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("platforms")]
    public class Platform : BaseModel
    {
        [PrimaryKey("platform_id")]
        public int PlatformID { get; set; }

        [Column("platform_name")]
        public string PlatformName { get; set; } = string.Empty;

        [Column("api_key")]
        public string APIKey { get; set; } = string.Empty;
    }
}
