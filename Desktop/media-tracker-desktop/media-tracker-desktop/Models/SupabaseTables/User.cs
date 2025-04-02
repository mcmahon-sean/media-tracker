using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("Users")]
    public class User : BaseModel
    {
        [PrimaryKey("Username")]
        public string Username { get; set; } = string.Empty;

        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Column("LastName")]
        public string LastName { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Password")]
        public string Password { get; set; } = string.Empty;
    }
}
