using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("mediatypes")]
    public class MediaType : BaseModel
    {
        [PrimaryKey("media_type_id")]
        public int MediaTypeID { get; set; }

        [Column("media_type")]
        public string MediaTypeName { get; set; } = string.Empty;
    }
}
