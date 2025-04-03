using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop.Models
{
        [Table("mediatypes")]
        public class MediaTypes : BaseModel
    {
        [PrimaryKey("media_type_id")]
        public int MediaTypeId { get; set; }
        [Column("media_type")]
        public string MediaType { get; set; } = string.Empty;
    }
}