using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models.SupabaseTables
{
    [Table("MediaTypes")]
    public class MediaType : BaseModel
    {
        [PrimaryKey("MediaTypeID")]
        public int MediaTypeID { get; set; }

        [Column("MediaType")]
        public string MediaTypeName { get; set; } = string.Empty;
    }
}
