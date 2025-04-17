using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("useraccounts")]
    public class UserAccount : BaseModel
    {
        [PrimaryKey("username")]
        public string Username { get; set; } = string.Empty;

        [Column("platform_id")]
        public int PlatformID { get; set; }

        [Column("user_platform_id")]
        public string UserPlatID { get; set; } = string.Empty;
    }
}
