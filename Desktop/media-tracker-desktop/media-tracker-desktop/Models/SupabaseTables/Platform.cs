using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("Platforms")]
    public class Platform : BaseModel
    {
        [PrimaryKey("PlatformID")]
        public int PlatformID { get; set; }

        [Column("PlatformName")]
        public string PlatformName { get; set; } = string.Empty;

        [Column("APIKey")]
        public string APIKey { get; set; } = string.Empty;
    }
}
