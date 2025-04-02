using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("UserAccounts")]
    public class UserAccount : BaseModel
    {
        [PrimaryKey("Username")]
        public string Username { get; set; } = string.Empty;

        [Column("PlatformID")]
        public int PlatformID { get; set; }

        [Column("UserPlatID")]
        public string UserPlatID { get; set; } = string.Empty;
    }
}
