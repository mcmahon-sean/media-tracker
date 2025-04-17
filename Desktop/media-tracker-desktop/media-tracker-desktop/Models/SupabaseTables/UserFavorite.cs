using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("userfavorites")]
    public class UserFavorite : BaseModel
    {
        [PrimaryKey("username")]
        public string Username { get; set; } = string.Empty;

        [Column("media_id")]
        public int MediaID { get; set; }

        [Column("favorites")]
        public bool Favorite { get; set; }
    }
}
