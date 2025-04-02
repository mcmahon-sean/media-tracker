using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("UserFavorites")]
    public class UserFavorite : BaseModel
    {
        [PrimaryKey("Username")]
        public string Username { get; set; } = string.Empty;

        [Column("MediaID")]
        public int MediaID { get; set; }

        [Column("Favorite")]
        public bool Favorite { get; set; }
    }
}
