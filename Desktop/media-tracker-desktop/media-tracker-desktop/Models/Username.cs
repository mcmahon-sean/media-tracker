using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
    // This is just a test table named 'Username' from my own supabase DB.

    [Table("Username")]
    public class Username : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("age")]
        public int Age { get; set; }

        [Column("testNumber")]
        public int TestNumber { get; set; }

    }
}
